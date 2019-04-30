using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    private Light thisObjectLight;
    public bool isActivated;
    public GameObject assiociatedObject;

    [Header("Light Values", order = 0)]
    [Space(10, order = 1)]
    public float maxIntensity;
    private float minIntensity;
    public float maxRange;
    private float minRange;
    public float lightGrowFactor;
    public float transformMoveFactor;
    public float rangeGrowFactor;
    [Header("Transform Values", order = 0)]
    [Space(10, order = 1)]
    public float maxYPos;
    private float minYPos;


    [Header("Vfx Components", order = 0)]
    [Space(10, order = 1)]
    public GameObject maxLightVfx;

    //Booléens d'activation
    private bool intensityIsMaxed;
    private bool rangeIsMaxed;
    private bool transformIsMaxed;

    private void Start()
    {
        thisObjectLight = GetComponent<Light>();
        minYPos = transform.position.y;
        minIntensity = thisObjectLight.intensity;
        minRange = thisObjectLight.range;
    }

    public void Loading()
    {
        if (!isActivated)
        {
            if (thisObjectLight.intensity < maxIntensity)
            {
                thisObjectLight.intensity += lightGrowFactor;
            }
            else
            {
                intensityIsMaxed = true;
            }
            if (transform.position.y < maxYPos)
            {
                transform.Translate(Vector3.up * transformMoveFactor);
            }
            else
            {
                transformIsMaxed = true;
            }
            if (thisObjectLight.range < maxRange)
            {
                thisObjectLight.range += rangeGrowFactor;
            }
            else
            {
                rangeIsMaxed = true;
            }
            if (rangeIsMaxed && transformIsMaxed &&  intensityIsMaxed)
            {
                Activation();
            }
        }
    }
    /*public void Desactivation()
    {
        if (!isActivated)
        {
            if (light.intensity > minIntensity)
            {
                light.intensity -= lightGrowFactor;
            }
            if (transform.position.y > minYPos)
            {
                transform.Translate(Vector3.down * transformMoveFactor);
            }
            if (light.range > minRange)
            {
                light.range -= rangeGrowFactor;
            }
        }
    }*/

    private void Activation()
    {
        Instantiate(maxLightVfx, transform.position, Quaternion.identity);
        isActivated = true;
        //Call Function Do something on the related object
    }
}
