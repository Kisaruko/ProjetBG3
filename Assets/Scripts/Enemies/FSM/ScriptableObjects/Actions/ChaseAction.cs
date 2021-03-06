﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    private bool isPulsating;
    public override void Act(StateController controller)
    {
        isPulsating = true;
        Chase(controller);
    }

    //Chase the player until he's dead by setting its Nav Mesh Agent destination to the player
    private void Chase(StateController controller)
    {
        StartPulsating(100, 1000, 4.5f, 2,controller);
        controller.animator.SetBool("Chasing", true);
        controller.animator.SetBool("Absorb", false);
        controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        //Debug.Log(controller.chaseTarget);
    }
    void StartPulsating(float minIntensity, float maxIntensity, float pulsateSpeed, float pulsateMaxDistance, StateController controller)
    {
        if (isPulsating)
        {
            float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulsateSpeed, pulsateMaxDistance));
            controller.trashMobStats.myMat.SetFloat("_EmissiveIntensity", emission);
        }
    }
}
