using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSanctuary : MonoBehaviour
{
    private GameObject playerLight;
    public bool getLightOnTrigger;
    public ParticleSystem feedBackPs;
    private BinaryLight binarylight;
    private ScaleOverTime scaleovertime;

    private void Start()
    {
        playerLight = FindObjectOfType<LightManager>().gameObject;
        binarylight = FindObjectOfType<BinaryLight>();
        InvokeRepeating("CheckIfPlayerGotLight", 0.1f, 1f);
        scaleovertime = this.transform.parent.GetComponentInChildren<ScaleOverTime>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11 && other.GetComponentInParent<BinaryLight>().gotLight == false)
        {
            if (Input.GetButtonDown("Attack"))
            {
                feedBackPs.Stop();
                scaleovertime.isScaling = false;
                scaleovertime.SetScaling();
                if (getLightOnTrigger)
                {
                    other.gameObject.GetComponentInParent<BinaryLight>().GetLight();
                }
                else
                {
                    playerLight.transform.parent = null;
                    playerLight.transform.position = transform.position + Vector3.up;
                }
            }
        }
    }
    private void CheckIfPlayerGotLight()
    {
        if(binarylight.gotLight)
        {
            feedBackPs.Stop();
            scaleovertime.isScaling = false;
            scaleovertime.SetScaling();
        }
        else
        {
            feedBackPs.Play();
            scaleovertime.isScaling = true;
            scaleovertime.SetScaling();
        }
    }
}
