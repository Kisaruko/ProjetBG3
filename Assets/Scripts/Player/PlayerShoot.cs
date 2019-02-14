using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour

{
    public GameObject ref_bullet;
    private bool canShootAgain = true;
    public float coolDown = 0.1f;
    public float time = 1f;
    public float rotationSpeed = 60f;
    public float recoilStrength;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Shooting();
    }
    void Shooting()
    {
        if ((Input.GetAxisRaw("Horizontal2") != 0 || Input.GetAxisRaw("Vertical2") != 0) && time >= coolDown)
        {
            
           rb.AddForce((transform.forward * -1) * recoilStrength, ForceMode.Impulse);
           Instantiate(ref_bullet, transform.position, Quaternion.identity);
           canShootAgain = false;
           time = 0f;
        }

        if (time < coolDown)
        {
            time = time + 0.01f;
        }

        if (time > coolDown)
        {
            canShootAgain = true;
        }

    }
}
