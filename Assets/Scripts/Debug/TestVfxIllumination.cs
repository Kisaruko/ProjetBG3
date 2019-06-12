using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVfxIllumination : MonoBehaviour
{
    public GameObject vfxIllu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(vfxIllu, transform.position, Quaternion.identity);
        }
    }
}
