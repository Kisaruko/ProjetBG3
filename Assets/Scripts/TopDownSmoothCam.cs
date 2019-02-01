using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSmoothCam : MonoBehaviour
{
    public bool followTarget = true;
    private GameObject target;
    [SerializeField] float smoothSpeed =0.0125f;
    [SerializeField] Vector3 offset;
    private RaycastHit hit;
    public bool isBehindWall;
    public float maxRange = 2.0f;
    private Vector3 baseOffset;
    public Vector3 behindWallPos;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        baseOffset = offset;
    }
    void Update()
    {
        MoveCam();
        ClippingCheck();
        AjustCam();
    }
    void MoveCam()
    {
        if(followTarget == true)
        {
             Vector3 desiredPosition = target.transform.position + offset;
             Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
             transform.position = smoothedPosition;
             transform.LookAt(target.transform);
        }
    }
    void ClippingCheck()
    {
            Vector3 playerPos = target.transform.position;
            Debug.DrawRay(playerPos, transform.position - playerPos, Color.black);

        if (Physics.Raycast(playerPos, transform.position - playerPos, out hit, maxRange))
        {
            isBehindWall = true;
        }
        else
        {
            isBehindWall = false;
        }
        
    }
    void AjustCam()
    {
        if(isBehindWall == true)
        {
            offset = Vector3.Lerp(offset,behindWallPos,smoothSpeed);
        }
        else
        {
            offset = Vector3.Lerp(offset, baseOffset, smoothSpeed);
        }
    }
 }

