using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoRippleEffect : MonoBehaviour
{
    public float timeBeforeDoRipple;

    private void Start()
    {
        Invoke("DoEmission", timeBeforeDoRipple);
    }
    private void DoEmission()
    {
        Camera.main.GetComponent<RippleEffect>().Emit();
    }
}
