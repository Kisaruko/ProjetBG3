using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    private bool isPulsating;
    private Material myMat;
    private Color baseColor;
    public override void Act(StateController controller)
    {
        myMat = controller.GetComponentInChildren<SkinnedMeshRenderer>().material;
        baseColor = myMat.GetColor("_EmissiveColor");
        isPulsating = true;
        Chase(controller);
    }

    //Chase the player until he's dead by setting its Nav Mesh Agent destination to the player
    private void Chase(StateController controller)
    {
        StartPulsating(100,1000,4.5f,2);
        controller.animator.SetBool("Chasing", true);
        controller.animator.SetBool("Absorb", false);
        controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        //Debug.Log(controller.chaseTarget);
    }
    void StartPulsating(float minIntensity, float maxIntensity, float pulsateSpeed, float pulsateMaxDistance)
    {
        if (isPulsating)
        {
            float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulsateSpeed, pulsateMaxDistance));
            myMat.SetColor("_EmissiveColor", Color.red);
            myMat.SetFloat("_EmissiveIntensity", emission);
        }
        else
        {
            myMat.SetColor("_EmissiveColor", baseColor);
        }
    }
}
