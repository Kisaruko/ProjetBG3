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
    void StartPulsating(float minIntensity, float maxIntensity, float pulsateSpeed, float pulsateMaxDistance)
    {
        float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulsateSpeed, pulsateMaxDistance));
        Color baseColor = myMats[1].color;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        myMats[1].SetColor("_EmissionColor", finalColor);
        myMats[0].SetColor("_EmissionColor", finalColor);
        troncMat.SetColor("_EmissionColor", finalColor);
    }
    private void Update()
    {
        if (startGlowing)
        {
            StartPulsating(1f,100f,2,2f);
        }
    }
    private void EnlightTree()
    {

    }
    private void EnlightSector()
    {

    }
}
