using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleParticleManager : MonoBehaviour
{
    public ParticleSystem[] particles;

    public void On()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }
    public void Off()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }
}
