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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
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

        if (isRotate)
        {
            float rotateSpeed =0f;
            bool chooseDirection = false;
            if (chooseDirection == false)
            {
                int direction = Random.Range(0, 2);
                if(direction == 0)
                {
                    rotateSpeed = 0.6f;
                }
                else
                {
                    rotateSpeed = 0.6f;
                }
                chooseDirection = true;
            }
            transform.RotateAround(player.transform.position, Vector3.up, rotateSpeed);
            transform.LookAt(player.transform);
        }

    }
}
