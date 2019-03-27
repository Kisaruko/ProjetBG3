using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godray : MonoBehaviour
{
    private PlayerBehaviour playerbehaviour;

    private void Start()
    {
        playerbehaviour = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerbehaviour.RegenLifeOnCac();
        }
    }
}
