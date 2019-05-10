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

    private void SetLightPos()
    {
        GetComponent<BinaryLight>().DropLight(0,0);
        if (isPlayerMustHaveLight || lightBeginPos == null)
        {
            GetComponent<BinaryLight>().GetLight();
        }
        if (!isPlayerMustHaveLight && lightBeginPos != null)
        {
            GetComponent<BinaryLight>().LightObject.transform.position = lightBeginPos.position;
        }
        GetComponent<BinaryLight>().LightCanBeRegrabed();
        if (lightBeginPos != null)
        {
            Destroy(lightBeginPos.gameObject);
        }
        Destroy(this);
    }


}
