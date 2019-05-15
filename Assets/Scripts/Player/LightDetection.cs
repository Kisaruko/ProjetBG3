﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float range;
    public LayerMask mask;
    private Light light;
    public float rangeMultiplier;
    public ParticleSystem ps;
    public LayerMask exceptionLayer;
    private BinaryLight binaryLight;
    [Header("Vfx Attributes", order = 0)]
    [Space(10, order = 1)]
    public bool followTarget;
    public float particlesSpeed;
    ParticleSystem.Particle[] m_Particles;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    ParticleSystem.NoiseModule noise;
    ParticleSystem.TrailModule trail;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;
    private GameObject particlesTarget;
    public float stoppingRange=0.3f;

    #region unityMehods
    private void Start()
    {
        binaryLight = GameObject.Find("Player").GetComponent<BinaryLight>();
        light = GetComponentInChildren<Light>();
        emission = ps.emission;
        emission.enabled = false;
    }
    private void Update()
    {
        range = light.intensity * rangeMultiplier;
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, mask)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~mask)) // si le ray ne touche pas de mur
            {
                followTarget = true;
                hitcol.GetComponent<SwitchBehaviour>().playerLight = this.gameObject;
                hitcol.GetComponent<SwitchBehaviour>().Loading();
                if (hitcol.GetComponent<SwitchBehaviour>().isActivated == false)
                {
                    ps.trigger.SetCollider(0, hitcol.GetComponent<SphereCollider>());// get le collider du player qui doit détruire les particules

                    particlesTarget = hitcol.gameObject;
                    emission.enabled = true;
                }
            }
        }

        if(followTarget)
        {
            ParticlesGoToTarget();
        }
    }
    private void FixedUpdate()
    {
        if (binaryLight.isThrown)
        {
            StopObject();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    #endregion
    #region ParticlesMethods

    public void StopFollow()
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
        }*/
        // Applique les changements au particule system
        ps.SetParticles(m_Particles, numParticlesAlive);

    }

    private void StopObject()
    {
        if (Physics.CheckSphere(transform.position, stoppingRange,~exceptionLayer))
        {
            binaryLight.LightCanBeRegrabed();
        }
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
    #endregion
}