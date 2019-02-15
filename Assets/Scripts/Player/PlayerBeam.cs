using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    private Rigidbody rb;
    public float loadingTime;
    public float loadedTime;
    public GameObject bullet;
    public float recoilStrength;
    public float bulletSpeed;
    Vector3 lastInput;
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
            lastInput  = new Vector3(xInput, 0f, yInput);
        }
        else
        {
            if(loadingTime>= loadedTime)
            {
                if (GetComponent<PlayerBehaviour>().canShoot == true)
                {
                    GetComponent<PlayerBehaviour>().UseLifeOnShoot();
                    rb.AddForce((transform.forward * -1) * recoilStrength, ForceMode.Impulse);
                    GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                    clone.GetComponent<Rigidbody>().velocity = lastInput.normalized * bulletSpeed;
                }
            }
            loadingTime = 0f;
        }
    }
} 

