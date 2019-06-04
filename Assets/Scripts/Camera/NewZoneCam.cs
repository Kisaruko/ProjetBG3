using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewZoneCam : MonoBehaviour
{
    public float newCamHeight;
    public float newCamDistance;
    public float newCamAngle;
    private float oldCamHeight;
    private float oldCamDist;
    private float oldCamAngle;

    public float timeToMoveCam;

    private CameraBehaviour cameraBehaviour;

    private void Start()
    {
        cameraBehaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<CameraBehaviour>();
        oldCamHeight = cameraBehaviour.camHeight;
        oldCamDist = cameraBehaviour.camDistance;
        oldCamAngle = cameraBehaviour.xCamRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            DOTween.To(() => cameraBehaviour.xCamRotation, x => cameraBehaviour.xCamRotation = x, newCamAngle, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camHeight, x => cameraBehaviour.camHeight = x, newCamHeight, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camDistance, x => cameraBehaviour.camDistance = x, newCamDistance, timeToMoveCam);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            DOTween.To(() => cameraBehaviour.xCamRotation, x => cameraBehaviour.xCamRotation = x, oldCamAngle, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camHeight, x => cameraBehaviour.camHeight = x, oldCamHeight, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camDistance, x => cameraBehaviour.camDistance = x, oldCamDist, timeToMoveCam);
        }
    }
}
