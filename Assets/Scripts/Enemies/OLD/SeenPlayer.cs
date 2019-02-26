using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // si le joueur entre en collision avec cet objet
        {
            GetComponentInParent<EnemyBehaviour>().playerIsInRange = true; // l'ennemi a vu le joueur
        }
    }

}
