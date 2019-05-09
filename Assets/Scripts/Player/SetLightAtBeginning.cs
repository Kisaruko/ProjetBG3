using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightAtBeginning : MonoBehaviour
{
    public Transform lightBeginPos;
    public bool isPlayerMustHaveLight;
    void Start()
    {
        Invoke("SetLightPos", 0.05f);
    }
    private void FixedUpdate()
    {
        GetComponent<BinaryLight>().LightObject.GetComponent<Rigidbody>().velocity = Vector3.zero ;
    }
    private void SetLightPos()
    {
        GetComponent<BinaryLight>().DropLight();
        GetComponent<BinaryLight>().LightCanBeRegrabed();
        GetComponent<BinaryLight>().LightObject.transform.position = lightBeginPos.position;
        Destroy(lightBeginPos.gameObject);
        if(isPlayerMustHaveLight)
        {
            GetComponent<BinaryLight>().GetLight();
        }
        Destroy(this);
    }


}
