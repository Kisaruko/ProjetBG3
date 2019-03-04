using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> itemsList = new List<GameObject>();
    public Vector3 itemPositionWhenPickup;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Key")
        {
            Debug.Log("Je possède " + other.gameObject.name + " dans mon inventaire");
            itemsList.Add(other.gameObject);
            other.gameObject.transform.position = itemPositionWhenPickup;
        }
    }
}
