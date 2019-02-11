using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject ref_bullet;
    private bool canShootAgain = true;
    public float coolDown =0.1f;
    public float time = 1f;
    public Transform player;
    public float rotationSpeed =60f;

    //test
    public bool shootWithTrigger;
    public bool shootWithJoystick;
    public bool shootWithTriggerAndJoystick;
    //test

    private void Update()
    {
        Shooting();
    }
    void Shooting()
    {
        if(shootWithTrigger == true)
        {
            float triggerAxis = Input.GetAxis("Shoot");
            if (triggerAxis != 0 && time >= coolDown)
            {
                Instantiate(ref_bullet, transform.position, Quaternion.identity);
                canShootAgain = false;
                time = 0f;
            }
        }

        if (shootWithJoystick == true)
        {
            if ((Input.GetAxisRaw("Horizontal2") != 0 || Input.GetAxisRaw("Vertical2") != 0) && time >= coolDown)
            {
                Instantiate(ref_bullet, transform.position, Quaternion.identity);
                canShootAgain = false;
                time = 0f;
            }
        }
        if(shootWithTriggerAndJoystick == true)
        {
            float triggerAxis = Input.GetAxis("Shoot");
            if (triggerAxis != 0 && time >= coolDown)
            {
                Instantiate(ref_bullet, transform.position, Quaternion.identity);
                canShootAgain = false;
                time = 0f;
            }
        }
        if(time< coolDown)
        {
            time = time+ 0.01f;
        }
        if(time>coolDown)
        {
            canShootAgain = true;
        }
        
    }

}
