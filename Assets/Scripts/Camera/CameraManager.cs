using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float newCamHeight;
    public float newCamDistance;
    public float newSmoothSpeed;

    private CameraBehaviour cameraBehaviour;

    private void Start()
    {
        cameraBehaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<CameraBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si le joueur touche, ça set les valeurs de camera données au behaviour de la camera
        if(other.CompareTag("Player"))
        {
            cameraBehaviour.camHeight = newCamHeight;
            cameraBehaviour.camDistance = newCamDistance;
            cameraBehaviour.smoothSpeed = newSmoothSpeed;
        }
    }
}
