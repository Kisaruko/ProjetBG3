using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBehaviour : ActivableObjects
{
    public float maxUvOffset;
    public float minUvOffset;

    public float scrollFactor;
    private bool isActivated = false;

    public Renderer emissiveRootRenderer;
    private Material emissiveMaterial;

    private void Start()
    {
        emissiveMaterial = emissiveRootRenderer.material;
    }

    public override void Activate()
    {
        isActivated = true;
    }

    public override void Deactivate()
    {
        isActivated = false;
    }

    private void Update()
    {
        emissiveMaterial.mainTextureOffset = new Vector2(Mathf.Clamp(emissiveMaterial.mainTextureOffset.x, minUvOffset, maxUvOffset), emissiveMaterial.mainTextureOffset.y);

        if(isActivated)
        {
            emissiveMaterial.mainTextureOffset -= new Vector2(scrollFactor, 0.0f);
        }

        if(!isActivated)
        {
            emissiveMaterial.mainTextureOffset += new Vector2(scrollFactor, 0.0f);
        }
    }
}
