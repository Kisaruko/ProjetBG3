using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsInSwitchAbsorbRange")]
public class IsInSwitchAbsorbRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isInSwitchAbsorbRange = IsInSwitchAbsorbRange(controller);
        return isInSwitchAbsorbRange;
    }

    private bool IsInSwitchAbsorbRange(StateController controller)
    {
        if (controller.chaseTarget.GetComponent<SwitchBehaviour>() &&
            !controller.chaseTarget.GetComponent<SwitchBehaviour>().isAtMinimum &&
            Vector3.Distance(controller.transform.position, controller.chaseTarget.position) < controller.trashMobStats.absorbRange)
        {
            return true;
        }
        else
            return false;
    }
}