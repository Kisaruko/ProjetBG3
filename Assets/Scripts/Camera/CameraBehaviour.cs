using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public float camHeight = 10f;
    public float camDistance = 20f;
    public float camAngle = 60f;
    public float xCamRotation = 60f;
    public float smoothSpeed = 0.5f;

    private Vector3 refVelocity;



    // Start is called before the first frame update
    void Start()
    {
        HandleCamera();
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    protected virtual void HandleCamera()
    {
        if (!target)
        {
            Debug.Log("No target assigned to the camera");
            return;
        }

        //Build world position vector
        Vector3 worldPosition = (Vector3.forward * -camDistance) + (Vector3.up * camHeight);
        //Debug.DrawLine(target.position, worldPosition, Color.red);

        //Build rotated vector
        Vector3 rotatedVector = Quaternion.AngleAxis(camAngle, Vector3.right) * worldPosition;
        //Debug.DrawLine(target.position, rotatedVector, Color.green);

        //Move the position
        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotatedVector;
        //Debug.DrawLine(target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(xCamRotation, 0f, 0f));

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        if (target)
        {
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawSphere(target.position, 1.5f);
        }

        Gizmos.DrawSphere(transform.position, 1.5f);
    }
}
