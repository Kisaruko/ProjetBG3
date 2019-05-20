using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    #region Variables
    public Transform target;
    public float camHeight = 10f;
    public float camDistance = 20f;
    public float camAngle = 0f;
    public float xCamRotation = 60f;
    public float smoothSpeed = 0.5f;
    public float anticipationFactor = 10f;

    public bool followTarget;

    private Vector3 refVelocity;
    #endregion

    #region Main Methods
    private void Awake()
    {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Start()
    {
        HandleCamera();
    }

    private void LateUpdate()
    {
        if (followTarget)
        {
            HandleCamera();
        }
    }
    #endregion

    #region Custom Methods
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

        //Build anticipated vector
            //Variables du joystick gauche
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
            //Variables du joystick droit
        float xInputShot = Input.GetAxis("Horizontal2");
        float yInputShot = Input.GetAxis("Vertical2");

        Vector3 anticipatedVector = new Vector3(xInput, 0f, yInput) *  anticipationFactor;
        Vector3 anticipatedShotVector = new Vector3(xInputShot, 0f, yInputShot) * anticipationFactor;

        if (xInputShot != 0 || yInputShot != 0) //Override the anticipated vector with the shot vector when the player use right joystick
        {
            anticipatedVector = anticipatedShotVector;
        }

        //Move the position
        Vector3 flatTargetPosition = target.position;
        //flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotatedVector + anticipatedVector;
        //Debug.DrawLine(target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(xCamRotation, 0f, 0f));

    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        if (target)
        {
            //Draw a line from the camera to the target and draw a sphere to both objects
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawSphere(target.position, 1.5f);
        }

        Gizmos.DrawSphere(transform.position, 1.5f);
    }
}
