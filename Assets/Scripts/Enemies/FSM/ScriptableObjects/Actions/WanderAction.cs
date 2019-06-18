using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wander")]
public class WanderAction : Action
{
    public override void Act(StateController controller)
    {
        Wander(controller);
    }

    private void Wander(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.animator.SetBool("Chasing", false);
        controller.animator.SetBool("Absorb", false);
        controller.animator.SetBool("Attack", false);
    }
}
