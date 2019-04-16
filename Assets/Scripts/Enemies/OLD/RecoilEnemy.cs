using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RecoilEnemy : MonoBehaviour
{
    StateController controller;
    NavMeshAgent navMeshAgent;
    Animator animator;

    public float recoilVelocity;
    public float recoilTime;
    
    void Start()
    {
        controller = GetComponent<StateController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    public void TakeHit()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("TakeHit",true); //set l'anim
        StartCoroutine("RecoilTime");
        StartCoroutine("CoolDownAnimRecoil");

    }
    public IEnumerator RecoilTime()
    {
        Vector3 recoilDirection = (controller.eyes.position - controller.chaseTarget.position).normalized; //calcul de la direction du recul
        navMeshAgent.velocity = (recoilDirection * recoilVelocity); // calcule et execute le recul 
        yield return new WaitForSeconds(recoilTime);// attendre la durée du recul

        StopCoroutine("RecoilTime");// arrêt de la coroutine
    }
    private IEnumerator CoolDownAnimRecoil()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("TakeHit", false); //set l'anim
        StopCoroutine("CoolDownAnimRecoil");
    }
}
