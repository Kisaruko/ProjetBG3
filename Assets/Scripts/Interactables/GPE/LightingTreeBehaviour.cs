﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingTreeBehaviour : ActivableObjects
{
    /* [Header("GlowVariables", order = 0)]
     [Space(10, order = 1)]

     public bool startGlowing;
     public float minIntensity;
     public float maxIntensity;
     private Color baseColor;*/

    public bool isActivated;
    private Light thisObjectLight;
    public bool isLoading;
    public float maxIntensity;
    public float maxRange;
    public float lightGrowFactor;
    public float rangeGlowFactor;
    private bool intensityIsMaxed;
    private bool rangeIsMaxed;
    private Material[] myMats;
    private Material troncMat;
    public GameObject activationFx;
    public Transform lightTargetSpot;
    public float range;
    private bool increaseRange;
    public float sphereMaxRange;
    public float rangeIncreaseFactor;
    public float timeBeforeResetCam;
    private void Start()
    {
        thisObjectLight = GetComponentInChildren<Light>();
        myMats = GetComponentInChildren<MeshRenderer>().materials;
        troncMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public void Loading()
    {
        if (!isActivated)
        {
            isLoading = true;

            if (thisObjectLight.intensity < maxIntensity)
            {
                thisObjectLight.intensity += lightGrowFactor;
            }
            else
            {
                intensityIsMaxed = true;
            }
            if (thisObjectLight.range < maxRange)
            {
                thisObjectLight.range += rangeGlowFactor;
            }
            else
            {
                rangeIsMaxed = true;
            }
            if (rangeIsMaxed && intensityIsMaxed)
            {
                isActivated = true;
                EnlightTree();
            }
        }
    }
    #region glowBehaviour
    /*
    private void Start()
    {
        myMats = GetComponentInChildren<MeshRenderer>().materials;
        troncMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
        baseColor = myMats[1].color;
    }

    void StartPulsating(float minIntensity, float maxIntensity, float pulsateSpeed, float pulsateMaxDistance)
    {
        float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulsateSpeed, pulsateMaxDistance));
        Color baseColor = myMats[1].color;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        myMats[1].SetColor("_EmissionColor", finalColor);
        myMats[0].SetColor("_EmissionColor", finalColor);
        troncMat.SetColor("_EmissionColor", finalColor);
    }
    private void Update()
    {
        if (startGlowing)
        {
            StartPulsating(1f,100f,2,2f);
        }
    }*/
    #endregion
    private void EnlightTree()
    {
        Camera.main.GetComponentInParent<CameraBehaviour>().smoothSpeed = 0.1f;

        Camera.main.GetComponentInParent<CameraBehaviour>().SetNewParameters(35, 25, 45, 2f);

        Camera.main.GetComponentInParent<CameraBehaviour>().target = thisObjectLight.transform;
        CameraShake.Shake(0.05f, 0.2f);

        if (myMats[1] != null)
        {
            myMats[1].EnableKeyword("_EMISSION");
        }
        if (myMats[0] != null)
        {
            myMats[0].EnableKeyword("_EMISSION");
        }
        if (troncMat != null)
        {
            troncMat.EnableKeyword("_EMISSION");
        }
        isLoading = false;
        Instantiate(activationFx, transform.position, Quaternion.identity);
        isActivated = true;
        increaseRange = true;
        StartCoroutine("ResetCam");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SwitchBehaviour>() != null && other.GetComponent<SwitchBehaviour>().isActivated == false)
        {
            other.GetComponent<SwitchBehaviour>().Activation();
        }
        if (other.GetComponent<EmitWhenTrigger>() != null)
        {
            other.GetComponent<EmitWhenTrigger>().ActivateEmission();
        }
    }
    private void Update()
    {
        if (increaseRange == true && range <= sphereMaxRange)
        {
            GetComponent<SphereCollider>().radius = range;
            thisObjectLight.range = range;
            range += rangeIncreaseFactor;
        }
    }
    private IEnumerator ResetCam()
    {
        Camera.main.GetComponentInParent<CameraBehaviour>().target = FindObjectOfType<PlayerMovement>().transform;
        yield return new WaitForSeconds(1f);
        Camera.main.GetComponentInParent<CameraBehaviour>().smoothSpeed = 0;
        StopCoroutine("ResetCam");

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sphereMaxRange);
    }
}