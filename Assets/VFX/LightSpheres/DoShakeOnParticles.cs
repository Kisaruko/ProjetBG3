using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoShakeOnParticles : MonoBehaviour
{
    public float shakeDuration;
    public float shakeIntensity;
    public float timeBeforeShake;

    private void Start()
    {
        Invoke("DoShakyCam",timeBeforeShake);
    }
    private void DoShakyCam()
    {
        CameraShake.Shake(shakeDuration, shakeIntensity);
    }

}
