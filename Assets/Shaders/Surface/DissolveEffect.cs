using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public Material mat;
    public float dissolveSpeed;
    public bool dissolve;
    public bool ressolve;

    public void Dissolve()
    {
        if(dissolve == true)
        { 
            mat.SetFloat("_Level", Mathf.Lerp(mat.GetFloat("_Level"), 1f, dissolveSpeed)); // je dissolve l'objet     
        }
    }
    public void Ressolve()
    {
        if(ressolve == true)
        {
            mat.SetFloat("_Level", Mathf.Lerp(mat.GetFloat("_Level"), 0f, dissolveSpeed)); // je reconstruit l'objet
        }
    }
    private void Update()
    {
        Ressolve();
        Dissolve();
        if(Input.GetKeyDown(KeyCode.C)) // debug
        {
            ressolve = true;
            dissolve = false;

        }
        if (Input.GetKeyDown(KeyCode.X)) // debug
        {
            ressolve = false;
            dissolve = true;
        }
    }
}
