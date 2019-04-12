using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float upSpeed;
    private void Start()
    {
        Destroy(this.gameObject, 10f);

    }
    void Update()
    {
        transform.Translate(Vector3.up * upSpeed, Space.World);
    }
}
