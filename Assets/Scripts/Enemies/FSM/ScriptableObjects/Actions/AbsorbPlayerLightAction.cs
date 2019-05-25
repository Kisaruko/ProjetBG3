using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/AbsorbPlayerLight")]
public class AbsorbPlayerLightAction : Action
{
    private float timer;

    public override void Act(StateController controller)
    {
        Absorb(controller);
        NavMeshControl(controller);
    }

    private void Absorb(StateController controller)
    {
        timer += Time.deltaTime;
        if(timer >= controller.trashMobStats.absorbCooldown)
        {
            if(controller.chaseTarget.GetComponentInParent<LightManager>())
            {
                controller.chaseTarget.GetComponent<LightManager>().LightDecreasing(controller.trashMobStats.absorbFactor);
                timer = 0;
            }
        }
    }
    
    private void NavMeshControl(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.animator.SetBool("Chasing", false);
        controller.animator.SetBool("Absorb", true);
    }
}
