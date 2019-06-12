using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        controller.trashMobStats.visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(controller.transform.position, controller.trashMobStats.lookRange, controller.trashMobStats.targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.transform.position - controller.transform.position).normalized;
            if(Vector3.Angle(controller.transform.forward, dirToTarget) < controller.trashMobStats.lookAngle / 2)
            {
                float distToTarget = Vector3.Distance(controller.transform.position, target.position);
                if(!Physics.Raycast(controller.transform.position, dirToTarget, distToTarget, controller.trashMobStats.obstacleMask))
                {
                    if (target.GetComponent<SwitchBehaviour>() != null &&  !target.GetComponent<SwitchBehaviour>().isAtMinimum)
                    {
                        return true;
                    }
                    if (target.GetComponent<LightManager>())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
