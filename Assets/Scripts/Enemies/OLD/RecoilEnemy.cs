using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RecoilEnemy : MonoBehaviour
{
    StateController controller;
    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    Animator animator;
    Transform player;

    public float recoilVelocity;
    public float recoilTime;
    
    void Start()
    {
        controller = GetComponent<StateController>();
        //navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
    }
    public void TakeHit()
    {
        //navMeshAgent.isStopped = true;
        animator.SetBool("TakeHit",true); //set l'anim
        StartCoroutine("RecoilTime");
        StartCoroutine("CoolDownAnimRecoil");

    }
    public IEnumerator RecoilTime()
    {
        Vector3 recoilDirection = (transform.position - transform.position).normalized; //calcul de la direction du recul
        rb.velocity = (recoilDirection * recoilVelocity); // calcule et execute le recul 
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
