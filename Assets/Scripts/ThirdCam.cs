using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCam : MonoBehaviour
{
    public GameObject Player;
    public Vector3 CenterOffset;
    public float cameraSpeed = 1;
    public float maxRange = 2.0f;

    private RaycastHit hit;
    private bool isBehindWall = false;
    public float maxHeight;
    public float minHeight;
    void ClippingCheck()
    {
        Vector3 playerPos = Player.transform.position + CenterOffset;
        Debug.DrawRay(playerPos, transform.position - playerPos, Color.red);
        if (Physics.Raycast(playerPos, transform.position - playerPos, out hit, maxRange))
        {
            isBehindWall = true;
        }
        else
            isBehindWall = false;
    }

    void Update()
    {
            transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y,minHeight,maxHeight),transform.position.z);
            float xInput = -Input.GetAxisRaw("Horizontal2");
            float yInput = -Input.GetAxisRaw("Vertical2");
            float xInputD = Input.GetAxisRaw("Horizontal");
            float yInputD = -Input.GetAxisRaw("Vertical");

            if (xInput != 0.0f || yInput != 0.0f)
            {
                Quaternion q = new Quaternion
                {
                    eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)
                };
                Vector3 movement = q * new Vector3(xInput, yInput, 0.0f);
                transform.Translate(movement * cameraSpeed * Time.deltaTime, Space.World);

                /*if ((Mathf.Pow(cameraPos.x - playerPos.x,2) + Mathf.Pow(cameraPos.y - playerPos.y, 2) + Mathf.Pow(cameraPos.z - playerPos.z, 2)) < Mathf.Pow(maxRange, 2))
                {

                }*/

            }
            else
            {
                Quaternion q = new Quaternion
                {
                    eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)
                };
                Vector3 movementD = q * new Vector3(xInputD, 0.0f, yInputD);
                transform.Translate(movementD * cameraSpeed * Time.deltaTime, Space.World);
            }
            Vector3 cameraPos = transform.position;
            Vector3 playerPos = Player.transform.position + CenterOffset;
            Vector3 newCameraPos = ((cameraPos - playerPos).normalized * maxRange) + playerPos;

            transform.LookAt(playerPos);
            transform.position = newCameraPos;
            ClippingCheck();
            if (isBehindWall)
            {
                transform.position = hit.point;
            }
        }
    }
