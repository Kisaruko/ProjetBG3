using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public bool playerIsInRange = false;
    public float moveSpeed;
    private GameObject player;
    private float minDistanceToAttack =3f;
    public float attackRange;
    public float stength;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(playerIsInRange == true)
        {
            transform.LookAt(player.transform);
            rb.velocity = (transform.forward.normalized) * moveSpeed;
        }
        Attack();
    }

    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (minDistanceToAttack > distanceToPlayer && playerIsInRange == true)
        {
            playerIsInRange = false;
            StartCoroutine("Attacking");
        }
    }
    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, 1f))
        {
            if (hitcol.gameObject.tag == "Player")
            {
               // hitcol.gameObject.GetComponent<PlayerBehaviour>().LostLifePoint(strength);
            }
        }
    
        Debug.Log("Attack!!");
        yield return new WaitForSeconds(1f);
        playerIsInRange = true;
        

    }
}
