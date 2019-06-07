using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateThings : ActivableObjects
{
    private Transform thisTransform;
    private Vector3 startRotation;
    public Vector3 endRotation;

    private void Start()
    {
        thisTransform = transform;
        startRotation = transform.rotation.eulerAngles;
    }
    public override void Activate()
    {
        transform.DORotate(endRotation, 2f, RotateMode.Fast);
    }
    public override void Deactivate()
    {
        transform.DORotate(startRotation, 2f, RotateMode.Fast);
    }
}
