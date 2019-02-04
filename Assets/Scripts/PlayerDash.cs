using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float maxDashTime;
    public float dashSpeed;
    public float dashStoppingSpeed;

    private float currentDashTime;

    // Start is called before the first frame update
    void Start()
    {
        currentDashTime = maxDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Dash
        if (Input.GetButtonDown("Fire2"))
        {
            currentDashTime = 0f;
        }
        if(currentDashTime < maxDashTime)
        {
            this.transform.position += new Vector3(Input.GetAxis("Horizontal") * dashSpeed, 0.0f, Input.GetAxis("Vertical") * dashSpeed);
            currentDashTime += dashStoppingSpeed;
        }
    }
}
