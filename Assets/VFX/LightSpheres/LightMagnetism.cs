using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class LightMagnetism : MonoBehaviour
{
    #region Variables
    public GameObject player;
    public float lifeRegen;
    public float timeBeforeParticlesMoveAgain;
    public static int nbParticles;
    private Transform lanternSpot;
    public ParticleSystem ps;
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private float particleSpeed;
    private bool newMovement = false;
    public PlayerBehaviour playerBehaviour;
    public CapsuleCollider playerCollider;
    #endregion
    #region Main Methods
    private void Start()
    {
        player = GameObject.Find("Player"); // Get le spot ou les particules doivent aller
        ps = GetComponent<ParticleSystem>();
        lanternSpot = player.transform; // Get le spot ou les particules doivent aller
        playerCollider = player.GetComponentInChildren<CapsuleCollider>();
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        StartCoroutine("CallMagnetism"); // Appelle la coroutine
        particleSpeed = 1; // Set la vitesse des particules a 1
        nbParticles = ps.emission.GetBurst(0).maxCount;
        InitializeIfNeeded();
        
    }

    private void Update()
    {
        if(newMovement) // Si les particules ont changé de direction
        {
            ParticleMagnetism(); // Appelle la fonction de magnetisme au spot toute les frames
        }
    }

    private void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter); // nb de particules qui ont trigger

        for (int i = 0; i < numEnter; i++) // pour chaque particle qui ont trigger
        {
            ParticleSystem.Particle p = enter[i]; // crée le tableau
            playerBehaviour.RegenLifeOnCac();
            p.remainingLifetime = 0f; // destruction de la particle en mettant son lifetime a 0
            enter[i] = p; // ajoute au tableau
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter); // Applique les changements


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
        yield return new WaitForSeconds(timeBeforeParticlesMoveAgain);// wait 2 seconds
        ParticleMagnetism(); //Appelle la fonction magnetisme
        StopCoroutine("CallMagnetism"); //Arrete la coroutine
    }
    void InitializeIfNeeded()
    {
        if (m_System == null) // si le particle system n'est pas set
        {
            m_System = GetComponent<ParticleSystem>(); // get le particle system
            m_System.trigger.SetCollider(1,playerCollider);// get le collider du player qui doit détruire les particules
           
        }
        

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles) // Si pas de particules sont instantiées ou si le tableau de particule est inférieur au max de particules
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles]; // Set la taille de la list de particules au max des particules atteignable
    }

    #endregion
}

