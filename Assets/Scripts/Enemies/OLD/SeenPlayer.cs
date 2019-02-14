using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponentInParent<RecoilManager>().playerIsInRange = true;
        }
    }

}
