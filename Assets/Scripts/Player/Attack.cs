using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject fxAttack;
    public int strength;
    public float range;
    void DoAttack()
    {
        if (Input.GetButtonDown("Fire1")) // Si le joueur appuie sur l'input d'attack
        {
            Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
            foreach (Collider hitcol in Physics.OverlapSphere(transform.position+ transform.forward, range)) // Crée une sphère devant le joueur de radius range
            {
                if (hitcol.gameObject.tag == "Enemy") // pour chaque ennemi dans la sphere
                {
                    GetComponent<PlayerBehaviour>().RegenLifeOnCac(); // appelle la fonction regen de lumière
                    hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
                }
            }
        }
    }
    private void Update()
    {
        DoAttack();
    }
}
