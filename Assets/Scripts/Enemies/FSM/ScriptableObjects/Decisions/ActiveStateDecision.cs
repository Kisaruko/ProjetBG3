using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool chaseTargetIsActive = ActiveState(controller);
        return chaseTargetIsActive;
    }

    private bool ActiveState(StateController controller)
    {
        if (controller.chaseTarget.gameObject.activeSelf && 
            Vector3.Distance(controller.transform.position, controller.chaseTarget.position) < controller.trashMobStats.lookRange)
        {
            return true;
        }
        else
            return false;

    }
}