using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    //Set the destination to the Nav Mesh Agent
    private void Patrol(StateController controller)
    {
        int waypointStart = Random.Range(0, controller.wayPointList.Count);
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.navMeshAgent.isStopped = false;
        controller.animator.SetBool("Chasing", true);

        //If the Nav Mesh Agent has reached his target it will choose an other way point from a random list of waypoints
        if(controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            controller.nextWayPoint = Random.Range(0, controller.wayPointList.Count);
            //controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}
