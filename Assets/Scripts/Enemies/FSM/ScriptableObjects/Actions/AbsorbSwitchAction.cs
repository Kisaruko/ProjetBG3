using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/AbsorbSwitch")]
public class AbsorbSwitchAction : Action
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
        if (timer >= controller.trashMobStats.absorbCooldown)
        {
            if (controller.chaseTarget.GetComponent<SwitchBehaviour>())
            {
                controller.chaseTarget.GetComponent<SwitchBehaviour>().Unload();
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
