using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitWhenTrigger : MonoBehaviour
{
    private Material[] myMat;
    private MeshRenderer mesh;
    private SkinnedMeshRenderer skinnedMesh;
    public GameObject vfxShine;
    public GameObject vfxDestroy;
    private Light light;
    private Light light2;
    private Animator anim;
    [HideInInspector] public bool isActivated = false;
    public bool activateAtStart;
    public bool doCamShake = true;


    private void Start()
    {
        Invoke("SetEverything", 0.1f);
    }

    private void SetEverything()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            mesh = GetComponent<MeshRenderer>();
            myMat = mesh.materials;
        }
        if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
            myMat = skinnedMesh.materials;
        }
        if (GetComponentInChildren<Animator>() != null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        if (GetComponentInChildren<Light>() != null)
        {
            light = GetComponentInChildren<Light>();
            light.enabled = false;
        }
        if (GetComponent<Light>() != null)
        {
            light2 = GetComponent<Light>();
            light2.enabled = false;
        }
        if (activateAtStart)
        {
            ActivateEmission();
        }
        if (!activateAtStart)
        {
            DeactivateEmission();
        }
    }
    public void ActivateEmission()
    {
        if (anim != null)
        {
            anim.SetBool("IsActivated", true);
        }
        if (!isActivated)
        {
            if (doCamShake)
            {
                CameraShake.Shake(0.05f, 0.05f);
            }
            if (vfxShine != null)
            {
                Instantiate(vfxShine, transform.position, Quaternion.identity);
            }
            if (light != null)
            {
                light.enabled = true;
            }
            if (light2 != null)
            {
                light2.enabled = true;
            }
            foreach (Material mat in myMat)
            {
                mat.EnableKeyword("_EMISSION");
            }
            isActivated = true;
        }
        //Destroy(this);
    }

    public void DeactivateEmission()
    {
        if(isActivated)
        {
            if (anim != null)
            {
                anim.SetBool("IsActivated", false);
            }
            CameraShake.Shake(0.05f, 0.05f);
            if (light != null)
            {
                light.enabled = false;
            }
            if (light2 != null)
            {
                light2.enabled = false;
            }
            foreach (Material mat in myMat)
            {
                mat.DisableKeyword("_EMISSION");
            }
            
            isActivated = false;
        }
        
    }
}
