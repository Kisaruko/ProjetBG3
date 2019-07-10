using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    private float timer;

    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    //Set the destination to the Nav Mesh Agent
    private void Patrol(StateController controller)
    {
        timer += Time.deltaTime;
        controller.navMeshAgent.isStopped = false;
        controller.animator.SetBool("Chasing", true);
        if(timer >= controller.trashMobStats.patrolWanderTime)
        {
            Vector3 newPos = RandomNavCircle(controller.spawnPosition, controller.trashMobStats.patrolDist);
            controller.navMeshAgent.SetDestination(newPos);
            timer = 0;
        }

        if(controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            controller.animator.SetBool("Chasing", false);
        }
    }

    private Vector3 RandomNavCircle(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        return randDirection;
    }
}
