using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public static BulletBehaviour BulletManager;
    public float bulletSpeed = 50f;
    private Rigidbody rb;
    public GameObject ref_explode;
    private Vector3 pos;
    public float LifeTime;
    private GameObject player;
    public int bulletDamage = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // get le rigidbody
        player = GameObject.FindGameObjectWithTag("Player"); // find le player

        if (player.GetComponent<PlayerShoot>().enabled == true) // si le script playershoot est actif // DEBUG LIGNE A ENLEVER
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal2"), 0f, Input.GetAxis("Vertical2")).normalized * bulletSpeed; // Calcul de la velocité de la balle
        }
        Destroy(this.gameObject, LifeTime); // detruire la balle au bout de "lifetime" secondes
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Zone") // Si la collision n'est pas le joueur, ni l'ennemi, ni une zone("trigger")
        {
            Instantiate(ref_explode, transform.position, Quaternion.identity); // instantie le fx de touche qqchose
            // Destroy(this.gameObject);  // detruit l'objet
            GetComponent<SphereCollider>().enabled = false;// ligne a enlever c'était pour ne pas detruire le fx de la balle immédiatement 
            
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(ref_explode, transform.position, Quaternion.identity); // instantie le fx de touche qqchose
            other.GetComponent<EnemyLife>().LostLifePoint(bulletDamage); // appel la fonction de perte de pdv de l'ennemi
          //  Destroy(this.gameObject); // detruie l'objet
            GetComponent<SphereCollider>().enabled = false;// ligne a enlever c'était pour ne pas detruire le fx de la balle immédiatement 
        }
    }

}
