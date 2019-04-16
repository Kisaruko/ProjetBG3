using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMobAnimationEvents : MonoBehaviour
{
    StateController controller;

    private void Awake()
    {
        controller = GetComponentInParent<StateController>();
    }

    public void Attack()
    {
        // Draw une sphere devant l'ennemi de radius attackrange
        foreach (Collider hitcol in Physics.OverlapSphere(controller.eyes.position, controller.trashMobStats.attackRange))
        {
            if (hitcol.gameObject.CompareTag("Player"))//Pour chaque joueur dans la zone
            {
                if (controller.CheckIfCountDownElapsed(controller.trashMobStats.attackRate)) //Prevents the mob to hit several times
                {
                    controller.stateTimeElapsed = 0f;
                    if (controller.chaseTarget.GetComponent<PlayerBehaviour>().isInvicible == false)
                    {
                        GameManager.ShowAnImpact(0.3f);
                        CameraShake.Shake(0.1f, 0.2f);
                        controller.chaseTarget.GetComponent<PlayerMovement>().Recoil(controller.transform, controller.trashMobStats.attackForce); //Appelle la fonction recoil du joueur et inflige un recul de valeur recoilInflected
                        controller.chaseTarget.GetComponent<PlayerBehaviour>().TakeHit(controller.trashMobStats.attackDamage); // Appelle la fonction qui fait perdre des pdv au joueur , le joueur perd 'strength' pdv
                        Instantiate(controller.fxHit, controller.chaseTarget.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }

    public void StartCharge()
    {
        controller.navMeshAgent.isStopped = true;
        controller.navMeshAgent.velocity = controller.eyes.forward.normalized * controller.trashMobStats.moveSpeed * 5;
    }
    public void StopCharge()
    {
        controller.navMeshAgent.isStopped = false;
        controller.navMeshAgent.velocity = controller.eyes.forward.normalized * controller.trashMobStats.moveSpeed;
        controller.animator.SetBool("Attack", false);
    }

    public void StopAIMovement()
    {
        controller.navMeshAgent.isStopped = true;
    }

    public void StartAIMovement()
    {
        controller.navMeshAgent.isStopped = false;
    }
}
