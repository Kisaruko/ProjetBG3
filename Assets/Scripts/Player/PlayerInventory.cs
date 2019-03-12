using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Variables
    public List<GameObject> itemsList = new List<GameObject>(); //Créer une nouvelle liste qui stock les objets
    public Vector3 itemPositionWhenPickup;
    #endregion

    #region Main Methods
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Key")
        {
            Debug.Log("Je possède " + other.gameObject.name + " dans mon inventaire");
            itemsList.Add(other.gameObject); //Ajoute l'item avec le tag dans l'inventaire
            other.gameObject.transform.position = itemPositionWhenPickup; //Envoie l'objet dans l'inventaire à une position donnée dans le monde
        }
    }
    #endregion
}
