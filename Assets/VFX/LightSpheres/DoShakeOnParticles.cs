using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoShakeOnParticles : MonoBehaviour
{
    public float shakeDuration;
    public float shakeIntensity;
    public float timeBeforeShake;
    public bool stickOnPlayerLight =true;
    private Transform lightPos;

    private void Start()
    {
        lightPos = FindObjectOfType<LightManager>().transform;
        Invoke("DoShakyCam",timeBeforeShake);
    }
    private void DoShakyCam()
    {
        CameraShake.Shake(shakeDuration, shakeIntensity);
    }
    private void Update()
    {
        if(lightPos != null && stickOnPlayerLight)
        {
            transform.position = lightPos.position;
        }
    }

}
