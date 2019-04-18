using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryLight : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [Header("Light Attributes", order = 0)]
    [Space(10, order = 1)]
    public bool gotLight;
    public GameObject LightObject;
    public Transform lightAnchor;
    public bool isAimingLight;
    public bool reticule;
    [Header("Physics Attributes", order = 0)]
    [Space(10, order = 1)]
    public float ejectionForce;
    private Rigidbody lightRb;
    public float ejectionDistance;
    public float ejectionHeight;

    private void Start()
    {
        lightRb = LightObject.GetComponent<Rigidbody>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            isAimingLight = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DropLight();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetLight();
        }
    }
    public void DropLight()
    {
        gotLight = false;
        LightObject.transform.parent = null;
        lightRb.isKinematic = false;
        lightRb.useGravity = true;
        Vector3 ejectionDirection = new Vector3(Random.Range(ejectionDistance, -ejectionDistance), ejectionHeight, Random.Range(ejectionDistance, -ejectionDistance));
        lightRb.AddForce(ejectionDirection);
    }
    public void GetLight()
    {
        gotLight = true;
        lightRb.isKinematic = true;
        lightRb.useGravity = false;
        LightObject.transform.position = lightAnchor.position;
        LightObject.transform.parent = transform;
    }
    void ThrowLight()
    {
        playerMovement.moveSpeed = 0;

    }
}
