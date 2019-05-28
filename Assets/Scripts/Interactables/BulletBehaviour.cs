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
    public int bulletDamage = 1;

    private void Start()
    {
        CameraShake.Shake(0.2f, 1f);
        rb = GetComponent<Rigidbody>(); // get le rigidbody


        Destroy(this.gameObject, LifeTime); // detruire la balle au bout de "lifetime" secondes
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Zone") // Si la collision n'est pas le joueur, ni l'ennemi, ni une zone("trigger")
        {
            Instantiate(ref_explode, transform.position, Quaternion.identity); // instantie le fx de touche qqchose
             Destroy(this.gameObject);  // detruit l'objet
            
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(ref_explode, transform.position, Quaternion.identity); // instantie le fx de touche qqchose
            other.GetComponent<EnemyLife>().LostLifePoint(bulletDamage); // appel la fonction de perte de pdv de l'ennemi
            other.GetComponent<RecoilEnemy>().StartCoroutine("RecoilTime");
             Destroy(this.gameObject); // detruit l'objet
        }
    }

}
