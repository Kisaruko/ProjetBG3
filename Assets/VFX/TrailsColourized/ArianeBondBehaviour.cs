using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ArianeBondBehaviour : MonoBehaviour
{
    public Transform destination;
    private GameObject lightAnchor;
    private GameObject lightObject;
    public float vfxSpeed;
    public int collisions;
    private Rigidbody rb;
    

    /* !!!! CET OBJECT  DOIT SE GERER TOUT SEUL IL FAUT VIRER LES TRUCS DANS BINARYLIGHT*/
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lightObject = GameObject.Find("PlayerLight_v4-1");
        lightAnchor = GameObject.Find("PlayerLight");
        Destroy(this.gameObject, 10f);
        destination = lightObject.transform;
    }
    private void FixedUpdate()
    {
        if (destination != null)
        {
            Vector3 direction = destination.position - transform.position;
            rb.velocity = direction.normalized * vfxSpeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collisions++;
            if (collisions == 2)
            {
                destination = lightAnchor.transform;
            }
            CheckIfAllCollisionAreDone();
        }
    }
    private void CheckIfAllCollisionAreDone()
    {
        if (collisions >= 3)
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
