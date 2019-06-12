using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSanctuary : MonoBehaviour
{
    private GameObject playerLight;
    public bool getLightOnTrigger;

    private void Start()
    {
        playerLight = FindObjectOfType<LightManager>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.GetComponentInParent<BinaryLight>().gotLight == false)
        {
            if (getLightOnTrigger)
            {
                other.gameObject.GetComponentInParent<BinaryLight>().GetLight();
            }
            else
            {
                playerLight.transform.parent = null;
                playerLight.transform.position = transform.position+Vector3.up;
            }
        }
    }
}
