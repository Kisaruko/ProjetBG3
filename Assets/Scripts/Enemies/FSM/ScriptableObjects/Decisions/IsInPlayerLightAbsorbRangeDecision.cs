using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsInPlayerLightAbsorbRange")]
public class IsInPlayerLightAbsorbRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isInPlayerLightAbsorbRange = IsInPlayerLightAbsorbRange(controller);
        return isInPlayerLightAbsorbRange;
    }

    private bool IsInPlayerLightAbsorbRange(StateController controller)
    {
        if (controller.chaseTarget.parent == null &&
            Vector3.Distance(controller.transform.position, controller.chaseTarget.position) < controller.trashMobStats.absorbRange)
        {
            return true;
        }
        else
            return false;
    }
}
