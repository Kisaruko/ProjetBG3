using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsWanderingTimeFinished")]
public class IsWanderingTimeFinishDecision : Decision
{
    private float timer = 0;

    public override bool Decide(StateController controller)
    {
        bool isWanderingTimeFinished = IsWanderingTimeFinish(controller);
        return isWanderingTimeFinished;
    }

    private bool IsWanderingTimeFinish(StateController controller)
    {
        timer += Time.deltaTime;
        if (timer >= controller.trashMobStats.wanderTimeBeforePatrol)
        {
            timer = 0;
            return true;
        }
        else
            return false;
    }
}
