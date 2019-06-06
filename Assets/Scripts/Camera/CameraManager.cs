using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public float newCamHeight;
    public float newCamDistance;
    public float newCamAngle;

    public float timeToMoveCam;

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
            DOTween.To(() => cameraBehaviour.xCamRotation, x => cameraBehaviour.xCamRotation = x, newCamAngle, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camHeight, x => cameraBehaviour.camHeight = x, newCamHeight, timeToMoveCam);
            //cameraBehaviour.camHeight = newCamHeight;
            DOTween.To(() => cameraBehaviour.camDistance, x => cameraBehaviour.camDistance = x, newCamDistance, timeToMoveCam);
            //cameraBehaviour.camDistance = newCamDistance;
        }
    }
}
