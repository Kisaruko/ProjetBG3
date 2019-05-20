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
    private Rigidbody rb;
    private SimpleAI simpleIA;
    public GameObject fxHit;
    private bool isCharging = false;
    public float chargeSpeed;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        rb = GetComponentInParent<Rigidbody>();
        simpleIA = GetComponentInParent<SimpleAI>();
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
                    player.GetComponent<BinaryLight>().DropLight(400f,400f);
                }
                anim.SetBool("Attack", false);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.right, attackRange);
    }
    public void StartCharge()
    {
        this.transform.parent.rotation = Quaternion.LookRotation(player.transform.position - this.transform.position);

        isCharging = true;
    }
    public void StopCharge()
    {
        isCharging = false;
        rb.velocity= Vector3.zero;
        anim.SetBool("Attack", false);
    }
    private void FixedUpdate()
    {
        if(isCharging)
        {
            rb.velocity = (this.transform.parent.forward) * chargeSpeed; //Il avance toujours vers l'avant
        }
    }
}
