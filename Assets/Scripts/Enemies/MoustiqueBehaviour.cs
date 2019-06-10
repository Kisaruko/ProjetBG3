using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoustiqueBehaviour : MonoBehaviour
{
    public float speed;
    public bool generateRandomPivot;
    public Transform pivot;
    private Vector3 CoordonatesInVector;
    private Vector3 angleVelocity;
    private Rigidbody rb;
    public float pivotSpawnRange;
	void Start ()
    {
        angleVelocity = Vector3.up;
        rb = GetComponent<Rigidbody>();
        
        if(generateRandomPivot)
        {
            CoordonatesInVector = new Vector3(Random.Range(transform.position.x , transform.position.x + pivotSpawnRange), transform.position.y, transform.position.z);
                
        }
        else
        {
            CoordonatesInVector = pivot.position;
        }
	}

    void Movement()
    {
        transform.RotateAround(CoordonatesInVector, Vector3.up, speed * Time.deltaTime);
        Quaternion deltaRotation = Quaternion.Euler(angleVelocity * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

	void FixedUpdate ()
    {
        Movement();
	}
}
