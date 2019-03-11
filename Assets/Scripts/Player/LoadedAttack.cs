using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedAttack : MonoBehaviour
{
    private Animator anim;
    public bool isUnlocked;
    public float loadingTime;
    public float loadedTime = 2f;
    public float attackRange;
    public LayerMask attackSphereDetection;
    public LayerMask destroyableWalls;
    public int strength;
    public GameObject stealLightFxVariant;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void AttackTimer()
    {
        if (Input.GetButton("Attack"))
        {
            anim.SetBool("LoadCancel", false);

            loadingTime += Time.deltaTime; // augmente le temps de load en fonction du temps
            if (loadingTime >= loadedTime /5)
            {
                anim.SetBool("isCasting", true);
            }
        }
        if (Input.GetButtonUp("Attack"))
        {
            if (loadingTime >= loadedTime)
            {
                anim.SetBool("isAttackLoaded", true);
            }
            else
            {
                anim.SetBool("LoadCancel", true);
            }
            loadingTime = 0f;
            anim.SetBool("isCasting", false);
        }
    }
    public void DoChargedAttack()
    {
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, attackRange, attackSphereDetection))
        {
            hitcol.GetComponent<EnemyLife>().LostLifePoint(strength); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
            Instantiate(stealLightFxVariant, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
        }
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, attackRange,destroyableWalls))
        {
            Destroy(hitcol.gameObject);
        }
        anim.SetBool("isAttackLoaded", false);
        CameraShake.Shake(0.5f, 0.75f);
    }
    private void Update()
    {
        if (isUnlocked)
        {
            AttackTimer();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
