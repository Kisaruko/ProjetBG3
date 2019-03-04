using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponentInParent<EnemyBehaviour>().playerIsInRange = true;
        }
    }

}
