using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleOverTime : MonoBehaviour
{
    public bool isScaling;
    private MeshRenderer mesh;
    public GameObject ActivateVfx;
    private bool alreadyInstantiate;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        SetScaling();
    }

    public void SetScaling()
    {
        if (isScaling)
        {
            if (!alreadyInstantiate)
            {
                Instantiate(ActivateVfx, transform.position, Quaternion.identity);
               // transform.DOScale(3, 1).SetLoops(-1, LoopType.Yoyo);
                mesh.material.EnableKeyword("_EMISSION");
            }
            alreadyInstantiate = true;
        }
        else
        {
            if(alreadyInstantiate)
            {
                Instantiate(ActivateVfx, transform.position, Quaternion.identity);
              //  transform.DOScale(1, 1);
            }
            //  transform.DOScale(1f, 1f);
            mesh.material.DisableKeyword("_EMISSION");
            alreadyInstantiate = false;

        }
    }
}
