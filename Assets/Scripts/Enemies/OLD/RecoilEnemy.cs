using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilEnemy : MonoBehaviour
{
    private GameObject player;
    public float recoilVelocity;
    private Rigidbody rb;
    public float recoilTime;
    private MeshRenderer mesh;
    private Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = GetComponent<MeshRenderer>();
        anim = GetComponentInChildren<Animator>();
    }
    public void TakeHit()
    {
        GetComponent<TrashMobBehaviour>().playerIsDetected = false; //arrêt du mouvement du monstre
        GetComponent<TrashMobBehaviour>().isCharging = false; //arrêt du mouvement du monstre
        anim.SetBool("TakeHit",true); //set l'anim
        

        StartCoroutine("CoolDownAnimRecoil");

    }
    public IEnumerator RecoilTime()
    {
        Vector3 recoilDirection = transform.position - player.transform.position; //calcul de la direction du recul
        rb.velocity = (recoilDirection * recoilVelocity); // calcule et execute le recul 
        yield return new WaitForSeconds(recoilTime);// attendre la durée du recul

        GetComponent<TrashMobBehaviour>().playerIsDetected = true; // relance le mouvement normal du monstre
        StopCoroutine("RecoilTime");// arrêt de la coroutine
    }
    private IEnumerator CoolDownAnimRecoil()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("TakeHit", false); //set l'anim
        StopCoroutine("CoolDownAnimRecoil");
    }
}
