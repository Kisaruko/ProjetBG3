using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodrayParticles : MonoBehaviour
{
    private GameObject player;
    private Transform lanternSpot;
    private ParticleSystem ps;
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    ParticleSystem.NoiseModule noise;
    ParticleSystem.TrailModule trail;
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
    private bool followPlayer = false;
    public float particleSpeed = 1f;
    public PlayerBehaviour playerBehaviour;
    public CapsuleCollider playerCollider;


    #region MainMethods
    private void Start()
    {
        player = GameObject.Find("Player"); // Get le spot ou les particules doivent aller
        ps = GetComponent<ParticleSystem>();
        noise = ps.noise;
        trail = ps.trails;
        velocityOverLifetime = ps.velocityOverLifetime;
        lanternSpot = player.transform;
        playerCollider = player.GetComponentInChildren<CapsuleCollider>();
        InitializeIfNeeded();
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        noise.enabled = true;
        trail.ratio = 0;
        velocityOverLifetime.enabled = false;
    }

    private void Update()
    {
        if(followPlayer)
        {
            ParticlesGoToPlayer();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            trail.ratio = 1;
            noise.enabled = false;
            velocityOverLifetime.enabled = true;
            followPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            followPlayer = false;
            noise.enabled = true;
            velocityOverLifetime.enabled = false;
            trail.ratio = 0;
            ParticlesStopFollowPlayer();
        }
    }
    private void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter); // nb de particules qui ont trigger

        for (int i = 0; i < numEnter; i++) // pour chaque particle qui ont trigger
        {
            ParticleSystem.Particle p = enter[i]; // crée le tableau
            p.remainingLifetime = 0f; // destruction de la particle en mettant son lifetime a 0
            enter[i] = p; // ajoute au tableau
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter); // Applique les changements
    }
    #endregion

    #region CustomMethods
    void ParticlesGoToPlayer()
    {
        InitializeIfNeeded();

        // Get les particules en vie
        int numParticlesAlive = m_System.GetParticles(m_Particles);

        // Change seulement les particules en vie
        for (int i = 0; i < numParticlesAlive; i++)
        {
           // particleSpeed += 0.05f; // augmente la vitesse des particules toutes les frames
            Vector3 newVelocity = m_Particles[i].position - lanternSpot.position; // Calcule la direction vers le joueur
            m_Particles[i].velocity = (newVelocity * particleSpeed) * -1; // Set la velocité du particule concerné
        }

        // Applique les changements au particule system
        m_System.SetParticles(m_Particles, numParticlesAlive);

    }
    void ParticlesStopFollowPlayer()
    {
        InitializeIfNeeded();

        // Get les particules en vie
        int numParticlesAlive = m_System.GetParticles(m_Particles);

        // Change seulement les particules en vie
        for (int i = 0; i < numParticlesAlive; i++)
        {
            // particleSpeed += 0.05f; // augmente la vitesse des particules toutes les frames
            m_Particles[i].velocity = (Vector3.zero); // Set la velocité du particule concerné
        }

        // Applique les changements au particule system
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }
    void InitializeIfNeeded()
    {
        if (m_System == null) // si le particle system n'est pas set
        {
            m_System = GetComponent<ParticleSystem>(); // get le particle system
            m_System.trigger.SetCollider(1, playerCollider);// get le collider du player qui doit détruire les particules

        }


        if (m_Particles == null || m_Particles.Length < ps.main.maxParticles) // Si pas de particules sont instantiées ou si le tableau de particule est inférieur au max de particules
            m_Particles = new ParticleSystem.Particle[ps.main.maxParticles]; // Set la taille de la list de particules au max des particules atteignable
    }
    #endregion
}
