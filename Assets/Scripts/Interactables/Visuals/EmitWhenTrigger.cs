using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitWhenTrigger : MonoBehaviour
{
    private Material myMat;
    private MeshRenderer mesh;
    public GameObject vfxShine;
    private Light light;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        myMat = mesh.material;
        if(GetComponent<Light>() != null)
        {
            light = GetComponent<Light>();
        }
    }
    public void ActivateEmission()
    {
        if (vfxShine != null)
        {
            Instantiate(vfxShine, transform.position, Quaternion.identity);
        }
        if(light != null)
        {
            light.enabled = true;
        }
        myMat.EnableKeyword("_EMISSION");
        Destroy(this);
    }
}
