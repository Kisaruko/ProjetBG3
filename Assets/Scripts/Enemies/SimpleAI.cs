using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    private Transform playerTransform;
    private Transform playerLightTransform;
    private NavMeshAgent meshAgent;
    private Animator animator;

    [Header("Detection Variables", order = 0)]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //[HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public Transform currentTarget;

    [Header("Patrol Variables", order = 0)]
    public float wanderRadius;
    public float maxWanderTimer;
    private float wanderTimer;
    private float timer;
    private Vector3 spawnPosition;

    [Header("Absorb Variables", order = 0)]
    public float absorbRange;
    public float absorbCooldown;
    private float absorbTimer;
    public float absorbFactor;

    [Header("Attack Variables", order = 0)]
    public float lightEjectionDistance;
    public float lightEjectionHeight;

    [Header("Attack Variables", order = 0)]
    public GameObject succionVfx;
    private GameObject clone;

    private void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();
        playerLightTransform = FindObjectOfType<LightManager>().GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
        spawnPosition = this.transform.position;

        wanderTimer = Random.Range(1f, maxWanderTimer);
    }

    private void Update()
    {
        if (Detection())
        {
            FollowLight();
        }

        if (!Detection())
        {
            Patrol();
        }
    }

    private bool Detection()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //Store in an array all the collider with the targetMask

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform; //Get the transform of the target
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized; //Create the direction between enemies and player
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) //If the player is inside the angle range
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position); //Create a distance between player and enemies
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) //if the raycast do not hit a wall between player and enemies
                {
                    if (target.gameObject.GetComponent<SwitchBehaviour>() != null && target.GetComponent<SwitchBehaviour>().isAtMinimum == false)
                    {
                        visibleTargets.Add(target);
                        currentTarget = target;
                        return true; //return true to boolean in order to make something

                    }
                    if (target.gameObject.GetComponent<LightManager>() != null)
                    {
                        visibleTargets.Add(target);
                        currentTarget = target;
                        return true; //return true to boolean in order to make something

                    }
                }
            }
        }
        return false; //there's nothing to do here so it makes the bool to false
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layerMask) //Custom function in order to take a random position in the navmesh
    {
        Vector3 randDirection = Random.insideUnitSphere * dist; //Return a random point inside a sphere of 1 * the distance we want
        randDirection += origin; //Add the origin to the random point

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layerMask); //Find the closest point in a navmesh with a custom range

        return navHit.position; //return the vector 3
    }

    private void Patrol()
    {
        timer += Time.deltaTime;
        animator.SetBool("Chasing", true);
        if (timer >= wanderTimer) //if the timer is greater than the wanderTime
        {
            Vector3 newPos = RandomNavSphere(spawnPosition, wanderRadius, -1); //create a random point with the function above
            meshAgent.SetDestination(newPos); //Set the destination to the random point in the navmesh
            timer = 0;
        }

        if (meshAgent.velocity == Vector3.zero)
        {
            animator.SetBool("Chasing", false);
            // animator.SetBool("Absorb", false);
        }
    }

    Transform GetClosestTarget()
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform target in visibleTargets)
        {
            float dist = Vector3.Distance(currentPos, target.position);
            if (dist < minDist)
            {
                Debug.Log(dist);
                tMin = target;
                minDist = dist;
            }

        }
        return tMin;
    }

    private void FollowLight()
    {
        meshAgent.SetDestination(currentTarget.position);
        animator.SetBool("Chasing", true);

        if (Vector3.Distance(transform.position, currentTarget.position) < absorbRange)
        {
            meshAgent.isStopped = true;
            AbsorbLight();
            animator.SetBool("Chasing", false);
        }
        else
        {
            meshAgent.isStopped = false;
            animator.SetBool("Absorb", false);
        }
    }

    private void AbsorbLight()
    {
        absorbTimer += Time.deltaTime;

        if (absorbTimer >= absorbCooldown)
        {
            if (((1 << currentTarget.gameObject.layer) & targetMask) != 0)
            {
                if (currentTarget.GetComponent<Light>() != null && !currentTarget.GetComponent<SwitchBehaviour>().isAtMinimum) //Si c'est un réceptacle
                {
                    animator.SetBool("Absorb", true);
                    animator.SetBool("Chasing", false);
                    animator.SetBool("Attack", false);
                    currentTarget.GetComponent<SwitchBehaviour>().Unload(); //Unload le receptacle
                    clone = Instantiate(succionVfx, currentTarget.position, Quaternion.identity);
                    clone.GetComponent<SuckedLightBehaviour>().light = currentTarget;
                    clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = transform;
                    clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                }
                else
                {
                    if (currentTarget.parent != null) //Si la cible a un parent
                    {
                        if (currentTarget.GetComponentInParent<BinaryLight>().gotLight && currentTarget.GetComponentInParent<BinaryLight>().isInvicible == false) //Si il a la light et qu'il n'est pas invicible tu attaques
                        {
                            animator.SetBool("Absorb", false);
                            AttackPlayer(currentTarget);
                        }
                    }
                    else //Sinon tu absorbes la light du player
                    {
                        animator.SetBool("Absorb", true);
                        animator.SetBool("Chasing", false);
                        animator.SetBool("Attack", false);
                        currentTarget.GetComponent<LightManager>().LightDecreasing(absorbFactor);
                        clone = Instantiate(succionVfx, currentTarget.position, Quaternion.identity);
                        clone.GetComponent<SuckedLightBehaviour>().light = currentTarget;
                        clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = transform;
                        clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                    }
                }
            }
            else
            {
                animator.SetBool("Absorb", false);
            }
            absorbTimer = 0;
        }
    }
    public void DestroyClone()
    {
        if (clone != null)
        {
            Destroy(clone.gameObject);
        }
    }
    public void AttackPlayer(Transform target)
    {
        animator.SetBool("Absorb", false);
        animator.SetBool("Attack", true);
    }
}
