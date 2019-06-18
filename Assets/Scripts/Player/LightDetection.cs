using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    [Header("TriggerOptions", order = 0)]
    [Space(10, order = 1)]
    private bool isTransmitting;

    public float range;
    public LayerMask ObjectsThatCanBeTouched;
    private Light thisLight;

    private Transform actualVfxTarget;

    public ParticleSystem ps;
    public LayerMask exceptionLayer;
    private BinaryLight binaryLight;
    [Header("Vfx Attributes", order = 0)]
    [Space(10, order = 1)]
    public GameObject loader;
    public GameObject vfxTransmission;

    [Header("Object Attributes", order = 0)]
    [Space(10, order = 1)]
    public float stoppingRange = 0.3f;

    #region old
    /*  public bool followTarget;
      public float particlesSpeed;
      ParticleSystem.Particle[] m_Particles;
      List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
      ParticleSystem.NoiseModule noise;
      ParticleSystem.TrailModule trail;
      ParticleSystem.EmissionModule emission;
      ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
      private GameObject particlesTarget;*/
    #endregion
    private Rigidbody rb;
    //magnetism Variables
    public float magnetismSpeed;
    public float magnetismRangeDivider = 1;
    public bool activeMagnetism = false;

    public GameObject xButton;

    #region unityMehods
    private void Start()
    {
        binaryLight = GameObject.Find("Player").GetComponent<BinaryLight>();
        thisLight = GetComponentInChildren<Light>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        InputCheck();

        List<SwitchBehaviour> switchsList = new List<SwitchBehaviour>(); //crée une liste
        List<SwitchBehaviour> potentialTarget = new List<SwitchBehaviour>();
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, ObjectsThatCanBeTouched)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~ObjectsThatCanBeTouched)) // si le ray ne touche pas de mur
            {
                if (hitcol.GetComponent<SwitchBehaviour>() != null)
                {
                    if (hitcol.GetComponent<SwitchBehaviour>().isActivated == false)
                    {
                        potentialTarget.Add(hitcol.GetComponent<SwitchBehaviour>());
                        hitcol.GetComponent<SwitchBehaviour>().playerLight = this.gameObject;

                        SwitchBehaviour switchbehaviour = hitcol.GetComponent<SwitchBehaviour>();

                        if (isTransmitting)
                        {
                            CameraShake.Shake(0.1f, 0.02f);
                            if (loader != null)
                            {
                                loader.SetActive(true);
                            }
                            hitcol.GetComponent<SwitchBehaviour>().Loading();
                            switchsList.Add(switchbehaviour);
                            int index = Random.Range(0, switchsList.Count);
                            actualVfxTarget = switchsList[index].transform;
                            GameObject clone = Instantiate(vfxTransmission, transform.position, transform.rotation);
                            clone.GetComponent<SuckedLightBehaviour>().light = transform;
                            clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                            clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = actualVfxTarget;
                        }
                    }

                }
                if (hitcol.GetComponent<EmitWhenTrigger>() != null)
                {
                    hitcol.GetComponent<EmitWhenTrigger>().ActivateEmission();
                }
                if (hitcol.gameObject.layer == 11 && transform.parent == null && binaryLight.isRegrabable == true && activeMagnetism == true)
                {
                    if (Vector3.Distance(transform.position, hitcol.transform.position) < range / magnetismRangeDivider)
                    {
                        rb.velocity = (hitcol.transform.position - transform.position).normalized * magnetismSpeed;
                    }
                }
            }
            if (loader != null)
            {
                if (switchsList.Count <= 0)
                {
                    loader.SetActive(false);
                }
            }
        }
        if (xButton != null)
        {
            if (potentialTarget.Count.Equals(0))
            {
                xButton.GetComponent<ButtonDisplayer>().Disappear();
            }
            else
            {
                xButton.GetComponent<ButtonDisplayer>().Appear();
            }
        }

    }
    private void InputCheck()
    {
        if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.Space))
        {
            isTransmitting = true;
        }
        if (Input.GetButtonUp("Attack") || Input.GetKeyUp(KeyCode.Space))
        {
            isTransmitting = false;
        }
    }
    private void FixedUpdate()
    {
        StopObject();
    }
    private void StopObject()
    {
        if (Physics.CheckSphere(transform.position, stoppingRange, ~exceptionLayer))
        {
            binaryLight.LightCanBeRegrabed();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range / magnetismRangeDivider);
    }
    #endregion
}
