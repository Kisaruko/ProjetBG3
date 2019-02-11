using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public static BulletBehaviour BulletManager;
    public float bulletSpeed = 50f;
    private Rigidbody rb;
    public GameObject ref_explode;
    private float frequency = 20f;
    private float magnitude = 0.5f;
    private Vector3 axis;
    private Vector3 pos;
    public static bool doZigzag= false;
    public static bool doubleSize = false;
    public static bool destroyOnImpact = true;
    public float LifeTime;
    private GameObject player;

    private void Start()
    {
        axis = transform.right - transform.forward;

        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<Shoot>().shootWithJoystick == true)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal2"), 0f, Input.GetAxis("Vertical2")).normalized * bulletSpeed;
        }
        if (player.GetComponent<Shoot>().shootWithTrigger == true || player.GetComponent<Shoot>().shootWithTriggerAndJoystick == true)
        {
            rb.velocity = player.transform.forward.normalized * bulletSpeed;
        }
        Destroy(this.gameObject, LifeTime);

        if(doubleSize == true)
        {

            GetComponent<SphereCollider>().radius = GetComponent<SphereCollider>().radius *3;
            GetComponentInChildren<ParticleSystem>().startSize = GetComponentInChildren<ParticleSystem>().startSize * 3;
        }
    }
    private void Update()
    {
        if (doZigzag == true)
        {
            pos = transform.position;
            transform.localPosition = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Bullet" && other.gameObject.tag !="Gun")
        {
            //Instantiate(ref_explode, transform.position, Quaternion.identity);
            if (destroyOnImpact == true)
            {
              //  Destroy(this.gameObject);
            }
        }
    }

}
