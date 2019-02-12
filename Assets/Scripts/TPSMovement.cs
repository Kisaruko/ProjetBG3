using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMovement : MonoBehaviour
{
    public float Speed = 1.0f;
    public float rotateSpeed = 1.0f;
    public Animator anim;
    public GameObject MainCamera;

    private Rigidbody rb;


    void Movement()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = -Input.GetAxisRaw("Vertical") *-1;
        Quaternion q = new Quaternion
        {
            eulerAngles = new Vector3(0, MainCamera.transform.rotation.eulerAngles.y, MainCamera.transform.rotation.eulerAngles.z)
        };
        Vector3 movement = q * new Vector3(xInput, 0.0f, yInput);
        transform.Translate(movement * Speed * Time.deltaTime, Space.World);
        if ((xInput > 0.1f || xInput < -0.1f) || (yInput > 0.1f || yInput < -0.1f))
            transform.rotation = Quaternion.LookRotation(movement);
    }
    private void Update()
    {
        Movement();
    }
}
