using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/PlayerHasLight")]
public class PlayerHasLightDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool playerHasLight = PlayerHasLight(controller);
        return playerHasLight;
    }

    private bool PlayerHasLight(StateController controller)
    {
        if (controller.chaseTarget.GetComponentInParent<BinaryLight>() && controller.chaseTarget.GetComponentInParent<BinaryLight>().gotLight)
        {
            return true;
        }
        else
            return false;
    }
}

