using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsInRange")]
public class IsInRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isInRange = IsInRange(controller);
        return isInRange;
    }

    private bool IsInRange(StateController controller)
    {
        //If the distance between the player and the monster is lesser than the attack range it returns true in order to change the state to ATTACK
        if (Vector3.Distance(controller.eyes.position, controller.chaseTarget.position) < controller.trashMobStats.attackRange)
        {
            return true;
        }
        else
            return false;
    }
}
