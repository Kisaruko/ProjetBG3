using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckedLightBehaviour : MonoBehaviour
{
    public bool isSucked;
    public Transform mobSuckingSpot;
    public Transform light;

    public float particlesSpeed;
    private ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        InitializeIfNeeded();

    }
    private void Update()
    {
        if (isSucked)
        {
            ParticlesGoToTarget();
            transform.position = light.position;
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
    void InitializeIfNeeded()
    {
        /* if (m_System == null) // si le particle system n'est pas set
         {
             m_System = GetComponent<ParticleSystem>(); // get le particle system
             m_System.trigger.SetCollider(1, playerCollider);// get le collider du player qui doit détruire les particules
         }*/


        if (m_Particles == null || m_Particles.Length < ps.main.maxParticles) // Si pas de particules sont instantiées ou si le tableau de particule est inférieur au max de particules
            m_Particles = new ParticleSystem.Particle[ps.main.maxParticles]; // Set la taille de la list de particules au max des particules atteignable
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
            Vector3 newVelocity = m_Particles[i].position - mobSuckingSpot.transform.position; // Calcule la direction vers le joueur
            m_Particles[i].velocity = (newVelocity * particlesSpeed) * -1; // Set la velocité du particule concerné
        }
        // Applique les changements au particule system
        ps.SetParticles(m_Particles, numParticlesAlive);

    }
}