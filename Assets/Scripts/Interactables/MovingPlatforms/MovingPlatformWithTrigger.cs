using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformWithTrigger : ActivableObjects
{
    public bool moveToEndPoint;
    private Rigidbody rb;
    public Transform endPoint;
    private Vector3 beginPoint;
    public float speed;
    private Vector3 direction;
    public List<string> StayOnTags;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        beginPoint = transform.position;
    }

    public override void Activate()
    {
        moveToEndPoint = true;
        direction = endPoint.position - transform.position;
    }
    public override void Deactivate()
    {
        moveToEndPoint = false;
        direction = beginPoint - transform.position;
    }

    private void FixedUpdate()
    {
        if (moveToEndPoint)
        {
            rb.velocity = direction * speed;
            if (Vector3.Distance(endPoint.position, transform.position) < 1f)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.velocity = direction * speed;
            if (Vector3.Distance(beginPoint, transform.position) < 1f)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach (string taggedTrigger in StayOnTags)
        {
            if (other.CompareTag(taggedTrigger))
            {
                other.gameObject.GetComponent<Rigidbody>().velocity = rb.velocity;
            }
        }
    }
}
