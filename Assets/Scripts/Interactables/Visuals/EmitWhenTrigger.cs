using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitWhenTrigger : MonoBehaviour
{
    private Material myMat;
    private MeshRenderer mesh;
    public GameObject vfxShine;
    public GameObject vfxDestroy;
    private Light light;

    [HideInInspector] public bool isActivated = false;
    public bool activateAtStart;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        myMat = mesh.material;
        if(GetComponent<Light>() != null)
        {
            light = GetComponent<Light>();
            light.enabled = false;
        }
        if(activateAtStart)
        {
            ActivateEmission();
        }
    }
    public void ActivateEmission()
    {
        if(!isActivated)
        {
            CameraShake.Shake(0.05f, 0.05f);
            if (vfxShine != null)
            {
                Instantiate(vfxShine, transform.position, Quaternion.identity);
            }
            if (light != null)
            {
                light.enabled = true;
            }
            myMat.EnableKeyword("_EMISSION");
            isActivated = true;
        }
        //Destroy(this);
    }

    public void DeactivateEmission()
    {
        if(isActivated)
        {
            CameraShake.Shake(0.05f, 0.05f);
            if (light != null)
            {
                light.enabled = false;
            }
            myMat.DisableKeyword("_EMISSION");
            isActivated = false;
        }
        
    }
}
