using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformManager : ActivableObjects
{
    private Transform thisTransform;
    public bool movePos = true;
    public bool moveRotation = true;
    public bool moveScale = true;
    
    [Header("Position Values", order = 0)]
    [Space(10, order = 1)]
    public Vector3 endPosition;
    private Vector3 startPosition;

    public float moveSpeed = 1;

    [Header("Rotation Values", order = 0)]
    [Space(10, order = 1)]
    public bool workWithNotchs;
    public Vector3 endRotation;
    private Vector3 startRotation;

    public float rotateSpeed = 1;

    [Header("Scale Values", order = 0)]
    [Space(10, order = 1)]
    public Vector3 endScale;
    private Vector3 startScale;

    public float scaleSpeed = 1;


    private bool canRotate = true;

    private void Start()
    {
        thisTransform = transform;
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
        startScale = transform.localScale;
    }

    public override void Activate()
    {
        if (movePos)
        {
            transform.DOMove(endPosition, moveSpeed);
        }
        if (moveScale)
        {
            transform.DOScale(endScale, scaleSpeed);
        }
        if (moveRotation && canRotate)
        {
            if (!workWithNotchs)
            {
                transform.DORotate(endRotation, rotateSpeed, RotateMode.Fast);
            }
            else
            {
                transform.DORotate(transform.rotation.eulerAngles - endRotation, rotateSpeed, RotateMode.Fast);
                canRotate = false;
                Invoke("CoolDown", rotateSpeed +0.1f);
            }
        }

        if(GetComponent<FMODUnity.StudioEventEmitter>())
        {
            SendMessage("Play");
        }
    }
    public override void Deactivate()
    {
        if (movePos)
        {
            transform.DOMove(startPosition, moveSpeed);
        }
        if (moveScale)
        {
            transform.DOScale(startScale, scaleSpeed);
        }
        if (moveRotation)
        {
            if (!workWithNotchs)
            {
                transform.DORotate(startRotation, rotateSpeed, RotateMode.Fast);
            }
        }
    }
    private void CoolDown()
    {
        canRotate = true;
    }

}
