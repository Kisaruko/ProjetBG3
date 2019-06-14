using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateBehaviour : ActivableObjects
{
    [FormerlySerializedAs("m_EulerSpeed")]
    public Vector3 EulerSpeed = Vector3.zero;
    public bool isActivated;
    void Update()
    {
        if (isActivated)
        {
            var euler = transform.rotation.eulerAngles;
            euler += EulerSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(euler);
        } 
    }
    public override void Activate()
    {
        isActivated = false;
    }
    public override void Deactivate()
    {
        isActivated = true;
    }

}
