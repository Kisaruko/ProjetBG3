using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBehaviour : ActivableObjects
{
    [Header("Tiling Values")]
    public float tilingX;
    public float tilingY;

    [Header("Offset values")]
    public float offsetY;
    public float maxUvOffsetX;
    public float minUvOffsetX;

    public float scrollFactor;
    private bool isActivated = false;

    public Renderer emissiveRootRenderer;
    private Material emissiveMaterial;

    private void Start()
    {
        emissiveMaterial = emissiveRootRenderer.material;
        emissiveMaterial.mainTextureOffset = new Vector2(minUvOffsetX, offsetY);
        emissiveMaterial.mainTextureScale = new Vector2(tilingX, tilingY);
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
        emissiveMaterial.mainTextureOffset = new Vector2(Mathf.Clamp(emissiveMaterial.mainTextureOffset.x, minUvOffsetX, maxUvOffsetX), emissiveMaterial.mainTextureOffset.y);

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
