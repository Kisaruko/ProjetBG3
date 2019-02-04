using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSmoothCam : MonoBehaviour
{
    public bool followTarget = true;
    private GameObject target;
    [SerializeField] float smoothSpeed = 0.0125f;
    [SerializeField] Vector3 offset;
    private RaycastHit hit;
    public bool isBehindWall;
    public float maxRange = 2.0f;
    private Vector3 baseOffset;
    public Vector3 behindWallPos;

    [Header("Camera Positioning")]
    private Vector3 targetDirection;
    public float xCameraPosOffset;
    public float zCameraPosOffset;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        baseOffset = offset;
    }

    void LateUpdate()
    {
        CameraPosition();
        ClippingCheck();
        AjustCam();
    }

    void MoveCam(Vector3 offsetCameraPosition)
    {
        if (followTarget == true)
        {
            Vector3 desiredPosition = target.transform.position + offset + offsetCameraPosition;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            //transform.LookAt(target.transform);
        }
    }

    void CameraPosition()
    {
        targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        //Check Direction to offset the camera position on x axis
        if(targetDirection.x < 0)
        {
            MoveCam(new Vector3(-xCameraPosOffset, 0.0f, 0.0f));
        }
        if(targetDirection.x > 0)
        {
            MoveCam(new Vector3(xCameraPosOffset, 0.0f, 0.0f));
        }

        //Check Direction to offset the position on z axis
        if (targetDirection.z < 0)
        {
            MoveCam(new Vector3(0.0f, 0.0f, -zCameraPosOffset));
        }
        if (targetDirection.z > 0)
        {
            MoveCam(new Vector3(0.0f, 0.0f, zCameraPosOffset));
        }
        else
        {
            MoveCam(Vector3.zero);
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
        if (isBehindWall == true)
        {

            offset = Vector3.Lerp(offset, behindWallPos, smoothSpeed);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(60, transform.eulerAngles.y, transform.eulerAngles.z), smoothSpeed);
        }
        else
        {
            offset = Vector3.Lerp(offset, baseOffset, smoothSpeed);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(60, transform.eulerAngles.y, transform.eulerAngles.z), smoothSpeed);

        }
    }
}

