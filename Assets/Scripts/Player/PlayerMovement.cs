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
    public bool isRecoiling = false;
    public float recoilDuration;

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

        if (isRecoiling == false)
        {
            if (xInput != 0 || yInput != 0 && isDashing == false)
            {
                isMoving = true;
                if (lookDirection2 == Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                }
                rb.velocity = new Vector3(xInput, 0f, yInput) * moveSpeed;
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
    }
    void Dashing()
    {
        if(Input.GetButtonDown("Dash") && isReadyToDash == true && isRecoiling == false)
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

    public void Recoil(Transform enemy, float recoilSpeed)
    {
        if (GetComponent<PlayerBehaviour>().isInvicible == false)
        {
            isRecoiling = true;
            Vector3 recoilDirection = (enemy.position - transform.position).normalized;
            rb.velocity = (recoilDirection * recoilSpeed) * -1;
            StartCoroutine("RecoilTime");
        }
    }
    IEnumerator RecoilTime()
    {
       yield return new WaitForSeconds(recoilDuration);
        isRecoiling = false;
        StopCoroutine("RecoilTime");
       
    }

}
