using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller)
    {
        //controller.navMeshAgent.isStopped = true;
        controller.animator.SetBool("Chasing", false);
        controller.animator.SetBool("Attack", true);
    }
}
