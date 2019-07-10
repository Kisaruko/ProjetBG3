using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchParticlesBehaviour : MonoBehaviour
{
    private ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
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
}
