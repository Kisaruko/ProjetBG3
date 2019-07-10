using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RippleState : MonoBehaviour
{

    public Material mymat;
    public float speed;
    private bool setAtStart = true;

    private Vector4 rippleOrigin = Vector4.zero;

    public Vector3 RippleOrigin { set { rippleOrigin = new Vector4(value.x, value.y, value.z, 0); } }

    private void Update()
    {
        if (setAtStart)
        {
            rippleOrigin.w = rippleOrigin.w + 1000;
        }
        setAtStart = false;
        rippleOrigin.w = Mathf.Min(rippleOrigin.w + (Time.deltaTime * speed), 1);
        mymat.SetVector("_RippleOrigin", rippleOrigin);
    }
}
