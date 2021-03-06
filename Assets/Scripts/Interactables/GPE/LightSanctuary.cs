﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSanctuary : MonoBehaviour
{
    private GameObject playerLight;
    [HideInInspector]public bool getLightOnTrigger =true; // this variable can be obsolete at a certain design point of the godray
    public ParticleSystem feedBackPs;
    private BinaryLight binarylight;
    private Transform player;
    public float rangeBeforeActivateEmissive;
    public GameObject getLightFx;
    public Color myColor;
    private Material myMat;
    private bool pulse;

    [Header("PulseOptions", order = 0)]
    [Space(10, order = 1)]
    public float minIntensity;
    public float maxIntensity;
    public float pulsateSpeed;
    public float pulsateMaxDistance;

    public GameObject xButton;

    private void Start()
    {
        playerLight = FindObjectOfType<LightManager>().gameObject;
        binarylight = FindObjectOfType<BinaryLight>();
        InvokeRepeating("CheckIfPlayerGotLight", 0.1f, 0.1f);
        player = GameObject.Find("Player").transform;
        myMat = GetComponentInChildren<MeshRenderer>().material;
        xButton = FindObjectOfType<ButtonDisplayer>().gameObject;
        myColor = myMat.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.GetComponentInParent<BinaryLight>().gotLight == false && binarylight.isRegrabable == true)
        {
            pulse = true;
            playerLight.GetComponent<LightDetection>().IsInAGodRay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            pulse = false;
            playerLight.GetComponent<LightDetection>().IsInAGodRay = false;
            xButton.GetComponent<ButtonDisplayer>().Disappear();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11 && other.GetComponentInParent<BinaryLight>().gotLight == false && binarylight.isRegrabable == true )
        {
            xButton.GetComponent<ButtonDisplayer>().Appear();
            if (Input.GetButtonDown("Attack"))
            {
                xButton.GetComponent<ButtonDisplayer>().Disappear();

                if (feedBackPs != null)
                {
                    feedBackPs.Stop();
                }
                if (getLightOnTrigger)
                {
                    other.gameObject.GetComponentInParent<BinaryLight>().GetLight();
                }
                else
                {
                    playerLight.transform.parent = null;
                    playerLight.transform.position = transform.position + Vector3.up;
                }
                if (getLightFx != null)
                {
                    Instantiate(getLightFx, playerLight.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void CheckIfPlayerGotLight()
    {
        if(binarylight.gotLight)
        {
            if (feedBackPs != null)
            {
                feedBackPs.Stop();
            }
            GetComponentInChildren<RootBehaviour>().Deactivate();
        }
        else
        {
            if (Vector3.Distance(transform.position+ transform.forward * 2, player.position)<rangeBeforeActivateEmissive)
            {
                GetComponentInChildren<RootBehaviour>().Activate();
                if (feedBackPs != null)
                {
                    feedBackPs.Play();
                }
            }
            else
            {
                GetComponentInChildren<RootBehaviour>().Deactivate();
                feedBackPs.Stop();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position+transform.forward*2, rangeBeforeActivateEmissive);
        Gizmos.color = Color.magenta;
    }
    void StartPulsating(float minIntensity, float maxIntensity, float pulsateSpeed, float pulsateMaxDistance)
    {
        float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulsateSpeed, pulsateMaxDistance));
        Color baseColor = myMat.color;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        myMat.SetColor("_EmissionColor", finalColor);
    }
    private void Update()
    {
        if (pulse)
        {
            StartPulsating(minIntensity, maxIntensity, pulsateSpeed, pulsateMaxDistance);
        }
        else
        {
            myMat.color = myColor;
        }
    }
}

