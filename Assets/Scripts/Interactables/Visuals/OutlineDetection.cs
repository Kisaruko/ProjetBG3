﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class OutlineDetection : MonoBehaviour
{
    private Transform lightTransform;
    private Outline outline;
    private SwitchBehaviour switchbehaviour;
    private PressurePlateBehaviour pressurePlateBehaviour;
    private bool startSettingOutline;

    private void Start()
    {
        lightTransform = GameObject.Find("PlayerLight_v4-1").transform;
        if (GetComponent<Outline>() != null)
        {
            outline = GetComponent<Outline>();
        }
        if (GetComponent<SwitchBehaviour>() != null)
        {
            switchbehaviour = GetComponent<SwitchBehaviour>();
        }
        if (GetComponentInParent<PressurePlateBehaviour>() != null)
        {
            pressurePlateBehaviour = GetComponentInParent<PressurePlateBehaviour>();
        }
        Invoke("WaitBeforeStartOutline", 0.5f);
    }

    private void WaitBeforeStartOutline()
    {
        startSettingOutline = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (startSettingOutline == true)
        {
            if (switchbehaviour != null && Vector3.Distance(transform.position, lightTransform.position) < 5f && switchbehaviour.isActivated == false)
            {
                outline.SetOutline();
            }
            if (pressurePlateBehaviour != null && Vector3.Distance(transform.position, lightTransform.position) < 5f)
            {
                outline.SetOutline();
            }
            if (Vector3.Distance(transform.position, lightTransform.position) > 5f)
            {
                outline.RemoveOutline();
            }
        }
    }
}
