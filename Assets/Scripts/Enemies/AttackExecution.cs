﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExecution : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    public float recoilInflincted;
    public int strength;
    public float attackRange;
    private Rigidbody rb;
    public TrashMobBehaviour trashmobbehaviour;
    public GameObject fxHit;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        rb = GetComponentInParent<Rigidbody>();
        trashmobbehaviour = GetComponentInParent<TrashMobBehaviour>();
    }

    public void AttackExecute()
    {
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.right, attackRange)) // Draw une sphere devant l'ennemi de radius attackrange
        {
            if (hitcol.gameObject.CompareTag("Player")) //Pour chaque joueur dans la zone
            {
                if (player.GetComponent<BinaryLight>().isInvicible == false)
                {
                    Instantiate(fxHit, hitcol.transform.position, Quaternion.identity);
                    GameManager.ShowAnImpact(0.3f);
                    CameraShake.Shake(0.1f, 0.2f);
                    if (player.GetComponent<BinaryLight>().gotLight == false)
                    {
                        Destroy(player.gameObject);
                    }
                    player.GetComponent<PlayerMovement>().Recoil(transform, recoilInflincted); //Appelle la fonction recoil du joueur et inflige un recul de valeur recoilInflected

                    player.GetComponent<BinaryLight>().TakeHit();
                    player.GetComponent<BinaryLight>().DropLight(100f,100f);
                }
                anim.SetBool("Attack", false);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.right, attackRange);
    }
    public void StartCharge()
    {
        trashmobbehaviour.isCharging = true;
    }
    public void StopCharge()
    {
        trashmobbehaviour.isCharging = false;
    }
}
