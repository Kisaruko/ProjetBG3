using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public bool controlsAreEnabled;
    public float sensitivity;
    public float distToGround;

    [Header("Movement Variables", order = 0)]
    public bool isMoving = false;
    public float moveSpeed;
    private Rigidbody rb;
    private float BaseSpeed;
    public float moveSpeedWhileAiming;
    private Animator anim;
    public bool isInRotation;
    public int isInRotationThreshold; //C'est l'angle minimum pour être considéré en rotation ou non
    public float rotationSpeed;

    [Header("GroundCheck Variables")]
    public float slopeRayHeight;
    public float steepSlopeAngle;
    public float slopeThreshold;

    [Header("Dash Variables")]
    public float lifeUsageOnDash;
    public bool isDashing;
    public bool isReadyToDash;
    public float dashingTime;
    public float coolDown;
    public float dashSpeed;
    Quaternion lastRotation;
    private BinaryLight binaryLight;
    private LightManager lightManager;
    public float dashDecreaseFactor;

    [Header("Upgrade Dash Variables")]
    public bool upgradeDashUnlocked;
    public float loadedDash;
    public float loadingDash;
    public int maxNumbersOfDash;


    [Header("Recoil Variables")]
    public bool isRecoiling = false;
    public float recoilDuration;

    [Header("Particles Variables")]
    public GameObject trailDashParticles;
    public ParticleSystem shinyBody;
    #endregion

    #region Main Methods
    private void Start()
    {
        binaryLight = GetComponent<BinaryLight>();
        lightManager = FindObjectOfType<LightManager>();
        rb = GetComponent<Rigidbody>();
        BaseSpeed = moveSpeed;
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (controlsAreEnabled)
        {
            Movement();
            DashDetection();
        }
    }
    #endregion

    #region Custom Methods
    public bool CheckMoveableTerrain(Vector3 position, Vector3 desiredDirection, float distance)
    {
        Ray myRay = new Ray(position, desiredDirection); // cast a Ray from the position of our gameObject into our desired direction. Add the slopeRayHeight to the Y parameter.

        RaycastHit hit;

        if (Physics.Raycast(myRay, out hit, distance))
        {
            if (hit.collider.CompareTag("Untagged")) // Our Ray has hit the ground
            {
                float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, hit.normal); // Here we get the angle between the Up Vector and the normal of the wall we are checking against: 90 for straight up walls, 0 for flat ground.
                float radius = Mathf.Abs(slopeRayHeight / Mathf.Sin(slopeAngle)); // slopeRayHeight is the Y offset from the ground you wish to cast your ray from.

                if (slopeAngle >= steepSlopeAngle * Mathf.Deg2Rad) //You can set "steepSlopeAngle" to any angle you wish.
                {
                    if (hit.distance - GetComponentInChildren<CapsuleCollider>().radius > Mathf.Abs(Mathf.Cos(slopeAngle) * radius) + slopeThreshold) // Magical Cosine. This is how we find out how near we are to the slope / if we are standing on the slope. as we are casting from the center of the collider we have to remove the collider radius.
                                                                                                                                                      // The slopeThreshold helps kills some bugs. ( e.g. cosine being 0 at 90° walls) 0.01 was a good number for me here
                    {
                        return true; // return true if we are still far away from the slope
                    }

                    return false; // return false if we are very near / on the slope && the slope is steep
                }

               // return true; // return true if the slope is not steep

            }

        }
        return true;
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, -transform.up, Color.yellow);

        return (Physics.Raycast(transform.position, -transform.up, distToGround + 0.01f));
    }

    void Movement()
    {
        //Input Logic
        float xInput = Input.GetAxis("Horizontal") * sensitivity; //Joystick gauche horizontal
        float yInput = Input.GetAxis("Vertical") * sensitivity; //Joystick gauche vertical
        float xInput2 = Input.GetAxis("Horizontal2"); //Joystick droit horizontal
        float yInput2 = Input.GetAxis("Vertical2"); //Joystick droit vertical

        //Calculating Direction
        Vector3 lookDirection = new Vector3(xInput, 0f, yInput); // direction du joystick gauche
        Vector3 lookDirection2 = new Vector3(xInput2, 0f, yInput2); // direction du joystick droit

        //Calculating Animation 
        float animSpeed = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        anim.SetFloat("Speed", animSpeed);

        //calculating Gravity
        float gravity;

        if (IsGrounded())
        {
            gravity = 0f;
        }
        else
        {
            gravity = -1f;
        }

        if (isRecoiling == false && isDashing == false) // si le joueur ne prend pas un recul
        {
            if (xInput >= 0.1f || xInput <= -0.1f || yInput >= 0.1f || yInput < -0.1f) // si le joueur bouge mais ne dash pas
            {
                isMoving = true;//il bouge
                anim.SetBool("isMoving", true);

                Quaternion smoothRotation = Quaternion.LookRotation(lookDirection);
                //transform.rotation = Quaternion.LookRotation(looksDirection, Vector3.up); // le joueur regarde en face de lui
                transform.rotation = Quaternion.Slerp(lastRotation, smoothRotation, rotationSpeed);

                if(Quaternion.Angle(transform.rotation, smoothRotation) >= isInRotationThreshold)
                {
                    //Debug.Log("I'm in rotation");
                    isInRotation = true;
                }
                else
                {
                    //Debug.Log("Im not in rotation");
                    isInRotation = false;
                }


                Vector3 Velocity = new Vector3(xInput, gravity, yInput);
                Velocity.Normalize();
                if (CheckMoveableTerrain(transform.position, new Vector3(Velocity.x, 0, Velocity.z), 0.1f)) // filter the y out, so it only checks forward... could get messy with the cosine otherwise.
                {
                    rb.velocity = Velocity * moveSpeed; // le joueur avance dans la direction du joystick gauche
                }
                lastRotation = transform.rotation; //Enregistre le dernier input du joueur pour qu'il regarde dans la dernière direction dans laquelle il allait
            }

            else  // si le joueur ne bouge pas
            {
                anim.SetBool("isMoving", false);
                isMoving = false;

                rb.velocity = new Vector3(0f, gravity, 0f) * moveSpeed;  // la vitesse du joueur est de 0

                transform.rotation = lastRotation; // le joueur regarde dans la dernière direction enregistrée
            }

            if (xInput2 >= 0.5f || xInput2 <= -0.5f || yInput2 >= 0.5f || yInput2 < -0.5f) // si le joueur touche le joystick droit
            {
                //transform.rotation = Quaternion.LookRotation(lookDirection2, Vector3.up); // il regarde dans la direction du joystick droit: ça override l'autre joystick
                //lastRotation = transform.rotation; //le joueur regarde dans la derniere direction de l'input
                //moveSpeed = moveSpeedWhileAiming;
            }
            else
            {
                //  moveSpeed = BaseSpeed; // reviens à la vitesse originelle
            }
        }
    }

    public void DashDetection()
    {
        if (Input.GetButtonDown("Dash") || Input.GetMouseButtonDown(1) && isReadyToDash == true && isRecoiling == false) // si le joueur peut dasher, qu'il ne subit pas de recul et qu'il appuie sur l'input
        {
            if (lightManager.canDash == true && binaryLight.gotLight == true) // si le joueur a assez de lumière pour dasher // Remplacer par le candash de binarylight
            {
                anim.SetBool("isDashing", true);
                lightManager.LightDecreasing(dashDecreaseFactor);
                lightManager.canDash = false;
                shinyBody.Play();
                Instantiate(trailDashParticles, transform.position, Quaternion.identity);

                isReadyToDash = false; // le joueur ne peut pas redasher
                isDashing = true; // le joueur est en train de dasher
                Dash();
                StartCoroutine("DashTime"); // on lance la coroutine du cooldown du dash
            }
        }
        if ((Input.GetButtonDown("Dash") && (lightManager.canDash == false || binaryLight.gotLight == false) && isDashing == false))
        {
            //dash Echec
            anim.SetBool("failDash", true);
            StartCoroutine("FailDashTime");
        }
    }
    private void Dash()
    {
        if (isDashing) //si le joueur est en train de dasher
        {
            //Use rigidbody for other dash movement in order to not pass through walls
            rb.velocity = transform.forward * dashSpeed;
        }
    }
    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(dashingTime); // le temps que le dash dure
        anim.SetBool("isDashing", false);
        isDashing = false; // le joueur ne dash plus
        yield return new WaitForSeconds(coolDown); //lance le cooldown
        isReadyToDash = true; // le joueur peut redasher
        StopCoroutine("DashTime");// stop la coroutine
    }
    IEnumerator FailDashTime()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(1f); // le temps que le dash dure
        anim.SetBool("failDash", false);
        moveSpeed = BaseSpeed;
        StopCoroutine("failDashTime");// stop la coroutine
    }
    public void Recoil(Transform enemy, float recoilSpeed)
    {
        if (binaryLight.isInvicible == false) // si le joueur n'est pas invincible // remplacer par inviciblité sur 
        {
            anim.SetBool("TakeHit", true);
            transform.rotation = lastRotation; // la rotation = le dernier input sur joystick enregistré
            isRecoiling = true; //le joueur recul
            Vector3 recoilDirection = (enemy.position - transform.position).normalized; // calcule la direction entre le player et l'ennemi
            rb.velocity = (recoilDirection * recoilSpeed) * -1; // fait reculer le player par rapport à l'ennemi
            StartCoroutine("RecoilTime");
        }
    }
    IEnumerator RecoilTime()
    {
        yield return new WaitForSeconds(recoilDuration); // attends 'recoilduration"
        isRecoiling = false; // le joueur ne recule plus
        anim.SetBool("TakeHit", false);

        StopCoroutine("RecoilTime"); // stop recule

    }
    #endregion
}

