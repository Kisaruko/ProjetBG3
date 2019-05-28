using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsInAttackRange")]
public class IsInAttackRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isInAttackRange = IsInAttackRange(controller);
        return isInAttackRange;
    }

    private bool IsInAttackRange(StateController controller)
    {
        if (controller.chaseTarget.parent != null &&
            Vector3.Distance(controller.transform.position, controller.chaseTarget.position) < controller.trashMobStats.attackRange)
        {
            return true;
        }
        else
            return false;
    }
}
