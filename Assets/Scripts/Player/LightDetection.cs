using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float range;
    public LayerMask ObjectsThatCanBeTouched;
    private Light light;
    // public float rangeMultiplier;
    private Transform actualVfxTarget;

    public ParticleSystem ps;
    public LayerMask exceptionLayer;
    private BinaryLight binaryLight;
    [Header("Vfx Attributes", order = 0)]
    [Space(10, order = 1)]
    /*  public bool followTarget;
      public float particlesSpeed;
      ParticleSystem.Particle[] m_Particles;
      List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
      ParticleSystem.NoiseModule noise;
      ParticleSystem.TrailModule trail;
      ParticleSystem.EmissionModule emission;
      ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
      private GameObject particlesTarget;*/
    public GameObject vfxTransmission;
    public float stoppingRange=0.3f;
    private Rigidbody rb;

    //magnetism Variables
    public float magnetismSpeed;
    public float magnetismRangeDivider= 1;

    #region unityMehods
    private void Start()
    {
        binaryLight = GameObject.Find("Player").GetComponent<BinaryLight>();
        light = GetComponentInChildren<Light>();
        //emission = ps.emission;
        //emission.enabled = false;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        List<SwitchBehaviour> switchsList = new List<SwitchBehaviour>(); //crée une liste

        // range = light.intensity * rangeMultiplier;
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, ObjectsThatCanBeTouched)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~ObjectsThatCanBeTouched)) // si le ray ne touche pas de mur
            {
                if (hitcol.GetComponent<SwitchBehaviour>() != null)
                {

                    // followTarget = true;
                    hitcol.GetComponent<SwitchBehaviour>().playerLight = this.gameObject;
                    hitcol.GetComponent<SwitchBehaviour>().Loading();

                    SwitchBehaviour switchbehaviour = hitcol.GetComponent<SwitchBehaviour>();

                    if (hitcol.GetComponent<SwitchBehaviour>().isActivated == false)
                    {
                        switchsList.Add(switchbehaviour);
                        int index = Random.Range(0, switchsList.Count);
                        actualVfxTarget = switchsList[index].transform;
                        GameObject clone = Instantiate(vfxTransmission, transform.position, transform.rotation);
                        clone.GetComponent<SuckedLightBehaviour>().light = transform;
                        clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                        clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = actualVfxTarget;
                        /* ps.trigger.SetCollider(0, hitcol.GetComponent<SphereCollider>());// get le collider du player qui doit détruire les particules
                         particlesTarget = hitcol.gameObject;
                         emission.enabled = true;*/
                    }
                }
                if(hitcol.GetComponent<EmitWhenTrigger>() != null)
                {
                    hitcol.GetComponent<EmitWhenTrigger>().ActivateEmission();
                }
                if (hitcol.gameObject.layer == 11 && transform.parent == null && binaryLight.isRegrabable == true) 
                {
                    if (Vector3.Distance(transform.position, hitcol.transform.position) < range / magnetismRangeDivider)
                    {
                        rb.velocity = (hitcol.transform.position - transform.position).normalized * magnetismSpeed;
                    }
                }
            }
            switchsList.Clear();
        }

    /*    if(followTarget)
        {
            ParticlesGoToTarget();
        }*/
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

        Gizmos.DrawWireSphere(transform.position, range/magnetismRangeDivider);

    }
    #endregion
    #region ParticlesMethods

  /*  public void StopFollow()
    {
        emission.enabled = false;
        followTarget = false;
    }
    void ParticlesGoToTarget()
    {
        InitializeIfNeeded();
        // Get les particules en vie
        int numParticlesAlive = ps.GetParticles(m_Particles);

        // Change seulement les particules en vie
        for (int i = 0; i < numParticlesAlive; i++)
        {
            // particleSpeed += 0.05f; // augmente la vitesse des particules toutes les frames
            Vector3 newVelocity = m_Particles[i].position - particlesTarget.transform.position +Vector3.down; // Calcule la direction vers le joueur
            m_Particles[i].velocity = (newVelocity * particlesSpeed) * -1; // Set la velocité du particule concerné
        }
      /*  for (int i = 0; i < numParticlesAlive/3; i++)
        {
            Vector3 newVelocity = m_Particles[i].position - particlesTarget.transform.position + Vector3.up*5; // Calcule la direction vers le joueur
            m_Particles[i].velocity = (newVelocity * particlesSpeed) * -1; // Set la velocité du particule concerné
        }
        // Applique les changements au particule system
        ps.SetParticles(m_Particles, numParticlesAlive);

    }
    */

    /*
    void InitializeIfNeeded()
    {
        if (ps == null) // si le particle system n'est pas set
        {
            ps = GetComponentInChildren<ParticleSystem>(); // get le particle system
        }


        if (m_Particles == null || m_Particles.Length < ps.main.maxParticles) // Si pas de particules sont instantiées ou si le tableau de particule est inférieur au max de particules
            m_Particles = new ParticleSystem.Particle[ps.main.maxParticles]; // Set la taille de la list de particules au max des particules atteignable
    }*/
    #endregion
}
