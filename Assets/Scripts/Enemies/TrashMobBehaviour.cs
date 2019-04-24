using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMobBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private GameObject player;
    public float moveSpeed;
    private float playerDetection;
    public bool playerIsDetected;
    public float minDistToAttack;
    public bool isCharging;
    public float rotationSpeed;
    private bool isRotate = false;

    [Header("VFX Stuff", order = 0)]
    public GameObject trashmobMesh;
    public ParticleSystem ps;
    public GameObject fxFading;
    private bool fxHasBeenUsed = false;
    private bool fxHasBeenUsed2 = false;
    void Start()
    {
        player = GameObject.Find("PlayerLight_v4-1");
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //trashmobMesh = GetComponentInChildren<GameObject>();
        //ps = GetComponentInChildren<ParticleSystem>();
    }
    private void Update()
    {
        Detection();
        RotateAroundPlayer();
        if (isCharging)
        {
            /*Vector3 pointToLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(pointToLook);*/
            Vector3 direction = player.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed);
            rb.velocity = (transform.forward.normalized) * moveSpeed * 5; //Il avance toujours vers l'avant
        }
    }
    void Detection()
    {
        if(playerIsDetected)
        {
            if (!isCharging)
            {
                anim.SetBool("Chasing", true);
                MoveTowardPlayer();
            }
        }       
    }
    void MoveTowardPlayer()
    {
      //  Vector3 pointToLook = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 direction = player.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        rb.velocity = (transform.forward) * moveSpeed; //Il avance toujours vers l'avant
        CheckIfPlayerCanBeAttacked();
    }
    void CheckIfPlayerCanBeAttacked()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <=  minDistToAttack)
        {
            playerIsDetected = false;
            anim.SetBool("Chasing", false);
            anim.SetBool("Attack", true);
            StartCoroutine("AttackRecovery");
        }
    }
    IEnumerator AttackRecovery()
    {
        yield return new WaitForSeconds(1f);
        playerIsDetected = true;
        anim.SetBool("Attack", false);
        anim.SetBool("Chasing", true);
        isRotate = true;
        yield return new WaitForSeconds(1.5f);
        isRotate = false;
        StopCoroutine("AttackRecovery");
    }
    void RotateAroundPlayer()
    {
        var emission = ps.emission;
  

        if (isRotate)
        {
            float rotateSpeed =0.6f;
            if (fxHasBeenUsed == false)
            {
                Instantiate(fxFading, transform.position, Quaternion.identity);
                fxHasBeenUsed = true;
                emission.enabled = true;
            }
            trashmobMesh.transform.localScale = Vector3.zero;
            transform.RotateAround(player.transform.position, Vector3.up, rotateSpeed);
            fxHasBeenUsed2 = false;
            transform.LookAt(player.transform);
        }
        if(!isRotate)
        {
            if(fxHasBeenUsed2 == false)
            {
                Instantiate(fxFading, transform.position, Quaternion.identity);
                fxHasBeenUsed2 = true;
            }
            emission.enabled = false;
            trashmobMesh.transform.localScale = Vector3.one;
            fxHasBeenUsed = false;
        }

    }
}

