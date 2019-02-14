using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    private Rigidbody rb;
    public float loadingTime;
    public float loadedTime;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        ShootBeam();
    }
    void ShootBeam()
    {
        float xInput = Input.GetAxis("Horizontal2");
        float yInput = Input.GetAxis("Vertical2");

        if (xInput != 0 || yInput!= 0)
        {
            loadingTime += Time.deltaTime;
        }
        else
        {
            if(loadingTime>= loadedTime)
            {
                Debug.Log("Grospew");
            }
            loadingTime = 0f;
        }
    }
} 

