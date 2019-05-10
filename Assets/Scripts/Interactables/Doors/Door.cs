using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Variables
    public GameObject attachedKey;
    private PlayerInventory playerInventory;
    #endregion

    #region Main Methods
    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(playerInventory.itemsList.Contains(attachedKey)) //Si le joueur possède la clé attaché à la porte dans son inventaire ça execute le code
        {
            Destroy(this.gameObject);
            Debug.Log("J'ai ouvert " + this.gameObject.name + " avec : " + attachedKey.name);
        }
    }
    #endregion
}
