using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public Transform player;
    public float rotateSpeed;
    public bool isRotating;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine("Rotating");
        StartRotate();
        Invoke("StopRotate", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.LookAt(player);
            transform.RotateAround(player.position, Vector3.up, rotateSpeed);
        }
    }
    void StartRotate()
    {
        isRotating = true;
    }
    void StopRotate()
    {
        isRotating = false;
    }
}
