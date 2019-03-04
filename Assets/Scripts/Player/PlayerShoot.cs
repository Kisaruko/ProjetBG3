using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour

{
    public GameObject ref_bullet;
 //   private bool canShootAgain = true;
    public float coolDown = 0.1f;
    public float time = 1f;
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
        if ((Input.GetAxisRaw("Horizontal2") != 0 || Input.GetAxisRaw("Vertical2") != 0) && time >= coolDown) // si le joueur touche l'input de tir et que le cooldown est passé
        {
            if (GetComponent<PlayerBehaviour>().canShoot == true) // si le joueur peut tirer , si il a assez de lumière
            {
                GetComponent<PlayerBehaviour>().UseLifeOnShoot(); // j'utilise de la vie pour tirer
                rb.AddForce((transform.forward * -1) * recoilStrength, ForceMode.Impulse); // le recul du tir
                Instantiate(ref_bullet, transform.position, Quaternion.identity);// j'instantie ma balle
         //       canShootAgain = false;
                time = 0f; // mon cd revient à zero
            }
        }

        if (time < coolDown)
        {
            time = time + 0.01f; // je calcule mon cooldown
        }

        if (time > coolDown)
        {
            //canShootAgain = true;
        }

    }
}
