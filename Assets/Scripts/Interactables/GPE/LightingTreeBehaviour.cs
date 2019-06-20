using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingTreeBehaviour : ActivableObjects
{
    private Material[] myMats;
    private Material troncMat;
    public bool startGlowing;

    public float minIntensity;
    public float maxIntensity;
    private Color baseColor;
    private void Start()
    {
        myMats = GetComponentInChildren<MeshRenderer>().materials;
        troncMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
        baseColor = myMats[1].color;
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (startGlowing)
        {

            float emission = Mathf.Lerp(minIntensity, maxIntensity, 1f);
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            myMats[1].EnableKeyword("_EMISSION");
            myMats[0].EnableKeyword("_EMISSION");
            troncMat.EnableKeyword("_EMISSION");
            /* myMats[1].SetColor("_EmissiveIntensity", finalColor);
             myMats[0].SetColor("_EmissiveIntensity", finalColor);*/
            troncMat.SetColor("_EmissiveIntensity", finalColor); 
        }
    }
    private void EnlightTree()
    {

    }
    private void EnlightSector()
    {

    }
}
