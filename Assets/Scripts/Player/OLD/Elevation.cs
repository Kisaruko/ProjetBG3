using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevation : MonoBehaviour
{
    private Rigidbody rb;
    private CustomGravity customgravity;
    private float baseGravity;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        customgravity = GetComponent<CustomGravity>();
        baseGravity = customgravity.gravityScale;
    }
    private void Update()
    {
        if(Input.GetButton("Fire3"))
        {
            rb.AddForce(Vector3.up * 100);
            customgravity.gravityScale = 0f;
        }
    }
}
