using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlowEmissiveOnButtonChange : MonoBehaviour, ISelectHandler
{
    public MeshRenderer meshRenderer;
    Material material;
    public float baseIntensity;
    public float maxIntensity;
    public float duration;

    private void Start()
    {
        material = meshRenderer.material;
    }

    public void OnSelect(BaseEventData eventData)
    {
        material.DOFloat(maxIntensity, "_EmissiveIntensity", duration);
        StartCoroutine("BaseIntensity");
    }

    IEnumerator BaseIntensity()
    {
        yield return new WaitForSeconds(duration);
        {
            material.DOFloat(baseIntensity, "_EmissiveIntensity", duration);
        }
    }
}
