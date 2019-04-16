using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        //Draw a sphere cast and if a player enters in it will get the transform of the chaseing target in order to chase this transform until death
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.trashMobStats.lookRange, Color.green);

        if (Physics.SphereCast(controller.eyes.position, controller.trashMobStats.lookSphereCastRadius, controller.eyes.forward, out hit,
            controller.trashMobStats.lookRange) && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            controller.navMeshAgent.isStopped = false;
            return true;
        }
        else
            return false;
    }
}
