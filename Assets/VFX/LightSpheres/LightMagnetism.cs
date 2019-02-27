﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class LightMagnetism : MonoBehaviour
{
#region Variables
    private Transform lanternSpot;
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    private float particleSpeed;
    private bool newMovement = false;
    public GameObject subEmitterPs;
#endregion
#region Main Methods
    private void Start()
    {
        lanternSpot = GameObject.FindGameObjectWithTag("Player").transform; // Get le spot ou les particules doivent aller
        StartCoroutine("CallMagnetism"); // Appelle la coroutine
        particleSpeed = 1; // Set la vitesse des particules a 1
        
    }

    private void Update()
    {
        if(newMovement) // Si les particules ont changé de direction
        {
            ParticleMagnetism(); // Appelle la fonction de magnetisme au spot toute les frames
        }
    }
    #endregion mainMethods
#region Custom Methods

    void ParticleMagnetism()
    {
        InitializeIfNeeded();

        // Get les particules en vie
        int numParticlesAlive = m_System.GetParticles(m_Particles); 

        // Change seulement les particules en vie
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particleSpeed += 0.05f ; // augmente la vitesse des particules toutes les frames
            Vector3 newVelocity = m_Particles[i].position - lanternSpot.position; // Calcule la direction vers le joueur
            m_Particles[i].velocity = (newVelocity * particleSpeed)*-1; // Set la velocité du particule concerné
        }

        // Applique les changements au particule system
        m_System.SetParticles(m_Particles, numParticlesAlive);
        
        newMovement = true; // Active l'update de changement de direction
    }

    IEnumerator CallMagnetism()
    {
        yield return new WaitForSeconds(2f);// wait 2 seconds
        ParticleMagnetism(); //Appelle la fonction magnetisme
        StopCoroutine("CallMagnetism"); //Arrete la coroutine
    }
    void InitializeIfNeeded()
    {
        if (m_System == null) // si le particle system n'est pas set
        {
            m_System = GetComponent<ParticleSystem>(); // get le particle system
            m_System.trigger.SetCollider(1, GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CapsuleCollider>());// get le collider du player qui doit détruire les particules
           
        }
        

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles) // Si pas de particules sont instantiées ou si le tableau de particule est inférieur au max de particules
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles]; // Set la taille de la list de particules au max des particules atteignable
    }

    #endregion
}
