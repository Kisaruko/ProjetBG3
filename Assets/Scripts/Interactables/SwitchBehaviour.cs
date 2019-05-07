using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchBehaviour : MonoBehaviour
{
    private Light thisObjectLight;
    public bool isActivated;
    public GameObject assiociatedObject;
    private Spektr.LightningRenderer fil;
    public UnityEvent activationEvent = new UnityEvent();
    public UnityEvent deactivateEvent = new UnityEvent();
    public UnityEvent loadingEvent = new UnityEvent();

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
    public GameObject playerLight;

    [Header("Vfx Components", order = 0)]
    [Space(10, order = 1)]
    public GameObject maxLightVfx;
    private MeshRenderer mesh;
    private Material[] materials;
    private Material myMat;
    //Booléens d'activation
    private bool intensityIsMaxed;
    private bool rangeIsMaxed;
    private bool transformIsMaxed;
    private bool receiverIsSet;
    private bool isLoading;

    private void Start()
    {
        thisObjectLight = GetComponent<Light>();
        minYPos = transform.position.y;
        minIntensity = thisObjectLight.intensity;
        minRange = thisObjectLight.range;
        fil = GetComponent<Spektr.LightningRenderer>();
        mesh = GetComponent<MeshRenderer>();
        materials = mesh.materials;
        myMat = materials[1];
    }
    private void Update()
    {
        if (!isActivated && isLoading)
        {
            if (Vector3.Distance(transform.position, playerLight.transform.position) > playerLight.GetComponent<LightDetection>().range*2)
            {
                fil.emitterTransform = transform;
                receiverIsSet = false;
                deactivateEvent.Invoke();
                playerLight.GetComponent<LightDetection>().StopFollow();
            }
        }
    }
    public void Loading()
    {
        if (!isActivated)
        {
            isLoading = true;
            loadingEvent.Invoke();

            if (thisObjectLight.intensity < maxIntensity)
            {
                thisObjectLight.intensity += lightGrowFactor;
            }
            else
            {
                intensityIsMaxed = true;
            }
            if (transform.localPosition.y < maxYPos)
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
            if (rangeIsMaxed && transformIsMaxed && intensityIsMaxed)
            {
                Activation();
            }

            if (!receiverIsSet)
            {
                fil.emitterTransform = playerLight.transform;
                
                receiverIsSet = true;
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
        myMat.EnableKeyword("_EMISSION");
        receiverIsSet = true;
        isLoading = false;
        Instantiate(maxLightVfx, transform.position, Quaternion.identity);
        isActivated = true;
        fil.emitterTransform = transform;
        activationEvent.Invoke();
        playerLight.GetComponent<LightDetection>().StopFollow();

    }
}