using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    private float timer;

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        timer += Time.deltaTime;
        if(timer >= controller.trashMobStats.attackCooldown)
        {
            controller.navMeshAgent.isStopped = true;
            controller.animator.SetBool("Chasing", false);
            controller.animator.SetBool("Absorb", false);
            controller.animator.SetBool("Attack", true);
            timer = 0;
        }
        
    }
}
