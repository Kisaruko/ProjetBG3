using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour
{
    public bool triggerWithLight= true;
    public GameObject door;
    //public bool triggerWithWeight;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerWithLight)
        {
            if (other.gameObject.GetComponent<LightManager>() != null)
            {
                Debug.Log("isLight"); // call function from associated door
            }
        }
    }

}
