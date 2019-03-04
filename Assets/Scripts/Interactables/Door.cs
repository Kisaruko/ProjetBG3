using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject attachedKey;
    public PlayerInventory playerInventory;

    private void OnCollisionEnter(Collision collision)
    {
        if(playerInventory.itemsList.Contains(attachedKey))
        {
            Destroy(this.gameObject);
            Debug.Log("J'ai ouvert " + this.gameObject.name + " avec : " + attachedKey.name);
        }
    }
}
