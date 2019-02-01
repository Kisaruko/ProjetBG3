using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving = false;
    [SerializeField] float moveSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector3 velocity = new Vector3(xInput,0f,yInput);
            
        if (xInput != 0 || yInput != 0)
        {
            isMoving = true;
            rb.velocity = velocity.normalized*moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }
    }
}
