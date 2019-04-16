using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    //Chase the player until he's dead by setting its Nav Mesh Agent destination to the player
    private void Chase(StateController controller)
    {
        controller.animator.SetBool("Chasing", true);
        controller.navMeshAgent.destination = controller.chaseTarget.position;
    }
}
