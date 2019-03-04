using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public bool playerIsInRange = false;
    public float moveSpeed;
    private GameObject player;
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
    }
}
