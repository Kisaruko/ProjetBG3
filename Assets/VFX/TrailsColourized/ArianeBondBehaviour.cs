using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArianeBondBehaviour : MonoBehaviour
{
    public Transform destination;
    private GameObject lightAnchor;
    private GameObject lightObject;
    public float vfxSpeed;
    public float rangeBeforeComeBack;
    private void Start()
    {
        lightObject = GameObject.Find("PlayerLight_v4-1");
        lightAnchor = GameObject.Find("PlayerLight");
        destination = lightObject.transform;
    }
    private void Update()
    {
        if (destination != null)
        {

            if(destination == lightAnchor.transform)
            {
                transform.DOMove(lightAnchor.transform.position, vfxSpeed);
                if (Vector3.Distance(transform.position, lightAnchor.transform.position) < rangeBeforeComeBack)
                {
                    destination = lightObject.transform;
                }
            }
            if (destination == lightObject.transform)
            {
                transform.DOMove(lightObject.transform.position, vfxSpeed);
                if (Vector3.Distance(transform.position, lightObject.transform.position) < rangeBeforeComeBack)
                {
                    destination = lightAnchor.transform;
                }
            }
        }
    }

}
