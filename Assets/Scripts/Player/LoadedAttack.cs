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
    public GameObject loadedAttackFx;
    public GameObject loadingFx;
    void Start()
    {
        anim = GetComponent<Animator>(); // get l'animator
    }
    void AttackTimer()
    {
        if (Input.GetButton("Attack")) // si le pj attack
        {
            anim.SetBool("LoadCancel", false); // remets a false par précaution

            loadingTime += Time.deltaTime; // augmente le temps de load en fonction du temps

            if (loadingTime >= loadedTime /5) // lance l'animation de cast 
            {
                loadingFx.SetActive(true); // active le fx de load
                anim.SetBool("isCasting", true); // le joueur cast
            }
        }
        if (Input.GetButtonUp("Attack")) // lache le bouton
        {

            if (loadingTime >= loadedTime) // check si le chargement est validé
            {
                anim.SetBool("isAttackLoaded", true); // le cast est chargé
            }
            else
            {
                anim.SetBool("LoadCancel", true); // le load est cancel
                loadingFx.SetActive(false); // desactive fx de load
            }
            loadingTime = 0f; // le temps est remis a 0
            anim.SetBool("isCasting", false); // le joueur ne cast plus
        }


    }
    public void DoChargedAttack()
    {
        Instantiate(loadedAttackFx, transform.position, Quaternion.identity); // instantie le fx de vol de light
        loadingFx.SetActive(false); //stop le fx de load
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
        loadingFx.SetActive(false);

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
