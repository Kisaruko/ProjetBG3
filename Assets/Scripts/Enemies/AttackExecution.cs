using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExecution : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    public float recoilInflincted;
    public int strength;
    public float attackRange;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void AttackExecute()
    {
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.right, attackRange)) // Draw une sphere devant l'ennemi de radius attackrange
        {
            if (hitcol.gameObject.CompareTag("Player")) //Pour chaque joueur dans la zone
            {
                player.GetComponent<PlayerMovement>().Recoil(transform, recoilInflincted); //Appelle la fonction recoil du joueur et inflige un recul de valeur recoilInflected
                player.GetComponent<PlayerBehaviour>().TakeHit(strength); // Appelle la fonction qui fait perdre des pdv au joueur , le joueur perd 'strength' pdv
                anim.SetBool("Attack", false);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position+transform.right, attackRange);
    }
}
