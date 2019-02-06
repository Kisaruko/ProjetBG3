using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving = false;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 offset;
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
        float xInput2 = Input.GetAxis("Horizontal2");
        float yInput2 = Input.GetAxis("Vertical2");
        Vector3 lookDirection = new Vector3(xInput,0f,yInput);
        Vector3 lookDirection2 = new Vector3(xInput2,0f,yInput2);


        if (xInput != 0 || yInput != 0)
        {
            if (GetComponent<Shoot>().shootWithTrigger == true)
            {
                isMoving = true;
                transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                rb.velocity = transform.forward * moveSpeed * Time.fixedDeltaTime;
            }
            if(GetComponent<Shoot>().shootWithJoystick == true || GetComponent<Shoot>().shootWithTriggerAndJoystick== true)
            {
                isMoving = true;
                transform.rotation = Quaternion.LookRotation(lookDirection2, Vector3.up);
                rb.velocity = new Vector3(xInput, 0f, yInput).normalized * moveSpeed * Time.fixedDeltaTime;
            }
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }
    }
}
