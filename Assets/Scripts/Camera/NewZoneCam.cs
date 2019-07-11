using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewZoneCam : MonoBehaviour
{
    public float newCamHeight;
    public float newCamDistance;
    public float newCamAngle;
    public float newSmoothSpeed;
    private float oldCamHeight;
    private float oldCamDist;
    private float oldCamAngle;
    private float oldSmoothSpeed;

    public float timeToMoveCam;

    private CameraBehaviour cameraBehaviour;

    private void Start()
    {
        Invoke("SetParameters", 3f);
    }

    private void SetParameters()
    {
        cameraBehaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<CameraBehaviour>();
        oldCamHeight = cameraBehaviour.camHeight;
        oldCamDist = cameraBehaviour.camDistance;
        oldCamAngle = cameraBehaviour.xCamRotation;
        oldSmoothSpeed = cameraBehaviour.smoothSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            DOTween.To(() => cameraBehaviour.xCamRotation, x => cameraBehaviour.xCamRotation = x, newCamAngle, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camHeight, x => cameraBehaviour.camHeight = x, newCamHeight, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camDistance, x => cameraBehaviour.camDistance = x, newCamDistance, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.smoothSpeed, x => cameraBehaviour.smoothSpeed = x, newSmoothSpeed, timeToMoveCam);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            DOTween.To(() => cameraBehaviour.xCamRotation, x => cameraBehaviour.xCamRotation = x, oldCamAngle, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camHeight, x => cameraBehaviour.camHeight = x, oldCamHeight, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.camDistance, x => cameraBehaviour.camDistance = x, oldCamDist, timeToMoveCam);
            DOTween.To(() => cameraBehaviour.smoothSpeed, x => cameraBehaviour.smoothSpeed = x, oldSmoothSpeed, timeToMoveCam);
        }
    }
}
