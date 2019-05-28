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
        Vector3 dirToTarget = (controller.chaseTarget.position - controller.transform.position).normalized;
        Quaternion newRot = Quaternion.LookRotation(dirToTarget, Vector3.up);
        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, newRot, Time.deltaTime * controller.trashMobStats.rotationSpeed);

        timer += Time.deltaTime;
        if (timer >= controller.trashMobStats.absorbCooldown)
        {
            if (controller.chaseTarget.GetComponent<SwitchBehaviour>())
            {
                controller.chaseTarget.GetComponent<SwitchBehaviour>().Unload();
                GameObject clone = Instantiate(controller.fxAbsorb, controller.chaseTarget.position, Quaternion.identity);
                clone.GetComponent<SuckedLightBehaviour>().light = controller.chaseTarget;
                clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = controller.eyes;
                clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
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
