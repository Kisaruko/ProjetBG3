using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godray : MonoBehaviour
{
    private PlayerBehaviour playerbehaviour;
    public bool rotatingToCenter;
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
    private void Update()
    {
        if(rotatingToCenter)
        transform.RotateAround(Vector3.zero, Vector3.up, 0.1f);
    }
}
