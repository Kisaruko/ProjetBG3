using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving = false;
    [SerializeField] float moveSpeed;
    public bool isDashing;
    public bool isReadyToDash;
    public float dashingTime;
    public float coolDown;
    public float dashSpeed;
    Quaternion lastRotation;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Movement();
        Dashing();
    }
    void Movement()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        float xInput2 = Input.GetAxis("Horizontal2");
        float yInput2 = Input.GetAxis("Vertical2");

        Vector3 lookDirection = new Vector3(xInput,0f,yInput);
        Vector3 lookDirection2 = new Vector3(xInput2,0f,yInput2);

        if (xInput != 0 || yInput != 0 && isDashing == false)
        {
                isMoving = true;
                if (lookDirection2 == Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                }
                rb.velocity = new Vector3(xInput,0f, yInput).normalized * moveSpeed;
                lastRotation = transform.rotation;
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
            transform.rotation = lastRotation;
            
        }
        if (lookDirection2 != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection2, Vector3.up);
            lastRotation = transform.rotation;
        }
    }
    void Dashing()
    {
        if(Input.GetButtonDown("Dash") && isReadyToDash == true )
        {
            if (GetComponent<PlayerBehaviour>().canDash == true)
            {
                GetComponent<PlayerBehaviour>().UseLifeOnDash();
                isReadyToDash = false;
                isDashing = true;
                StartCoroutine("DashTime");
            }

        }
        if(isDashing == true)
        {
            rb.velocity = transform.forward * dashSpeed;
        }
    }
    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(coolDown);
        isReadyToDash = true;
        StopCoroutine("DashTime");

    }
}
