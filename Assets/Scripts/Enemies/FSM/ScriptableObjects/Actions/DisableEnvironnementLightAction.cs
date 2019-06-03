using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/DisableEnvironnementLight")]
public class DisableEnvironnementLightAction : Action
{
    private float timer;

    public override void Act(StateController controller)
    {
        DisableEnvironnementLight(controller);   
    }

    private void DisableEnvironnementLight(StateController controller)
    {
        timer += Time.deltaTime;
        if(timer >= controller.trashMobStats.disableCooldown)
        {
            foreach (Collider hitcol in Physics.OverlapSphere(controller.transform.position, controller.trashMobStats.disableRange, controller.trashMobStats.objectsToDisable))
            {
                Vector3 toCollider = hitcol.transform.position - controller.transform.position;
                Ray ray = new Ray(controller.transform.position, toCollider);
                if (!Physics.Raycast(ray, toCollider.magnitude, ~controller.trashMobStats.objectsToDisable))
                {
                    if (hitcol.GetComponent<EmitWhenTrigger>() != null && hitcol.GetComponent<EmitWhenTrigger>().isActivated)
                    {
                        hitcol.GetComponent<EmitWhenTrigger>().DeactivateEmission();
                        GameObject clone = Instantiate(controller.fxAbsorb, hitcol.transform.position, Quaternion.identity);
                        clone.GetComponent<SuckedLightBehaviour>().light = hitcol.transform;
                        clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = controller.eyes;
                        clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                        timer = 0;
                    }
                }
            }
        }
        
    }
}
