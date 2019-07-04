using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightDiminution : MonoBehaviour
{
    public Light directional;
    private float baseIntensity;
    public float newIntensity;
    public float timeToFadeLight;
    private void Start()
    {
        if(directional != null)
        {
            baseIntensity = directional.GetComponent<Light>().intensity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && directional !=null)
        {
            DOTween.To(() => directional.intensity, x => directional.intensity = x, newIntensity, timeToFadeLight);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 && directional != null)
        {
            DOTween.To(() => directional.intensity, x => directional.intensity = x, baseIntensity, timeToFadeLight);
        }
    }
}
