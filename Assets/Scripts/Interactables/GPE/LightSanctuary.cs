﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSanctuary : MonoBehaviour
{
    private GameObject playerLight;
    public bool getLightOnTrigger;
    public ParticleSystem feedBackPs;
    private BinaryLight binarylight;
    private Transform player;
    public float rangeBeforeActivateEmissive;

    private void Start()
    {
        playerLight = FindObjectOfType<LightManager>().gameObject;
        binarylight = FindObjectOfType<BinaryLight>();
        InvokeRepeating("CheckIfPlayerGotLight", 0.1f, 0.1f);
        player = GameObject.Find("Player").transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11 && other.GetComponentInParent<BinaryLight>().gotLight == false)
        {
            if (Input.GetButtonDown("Attack"))
            {
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
            if (feedBackPs != null)
            {
                feedBackPs.Play();
            }
            if (Vector3.Distance(transform.position,player.position)<rangeBeforeActivateEmissive)
            {
                GetComponentInChildren<RootBehaviour>().Activate();
            }
            else
            {
                GetComponentInChildren<RootBehaviour>().Deactivate();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, rangeBeforeActivateEmissive);
        Gizmos.color = Color.magenta;
    }
}
