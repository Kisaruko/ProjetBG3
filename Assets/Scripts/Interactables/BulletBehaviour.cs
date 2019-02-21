using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public static BulletBehaviour BulletManager;
    public float bulletSpeed = 50f;
    private Rigidbody rb;
    public GameObject ref_explode;
    private Vector3 axis;
    private Vector3 pos;
    public float LifeTime;
    private GameObject player;
    public int bulletDamage = 1;

    private void Start()
    {
        axis = transform.right - transform.forward;

        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerShoot>().enabled == true)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal2"), 0f, Input.GetAxis("Vertical2")).normalized * bulletSpeed;
        }
        Destroy(this.gameObject, LifeTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Zone")
        {
            Instantiate(ref_explode, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(ref_explode, transform.position, Quaternion.identity);
            other.GetComponent<EnemyLife>().LostLifePoint(bulletDamage);
            Destroy(this.gameObject);
            
        }
    }

}
