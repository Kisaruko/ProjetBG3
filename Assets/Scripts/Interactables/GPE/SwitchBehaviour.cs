using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using cakeslice;

public class SwitchBehaviour : MonoBehaviour
{
    public bool activateAtStart;

    private Light thisObjectLight;
    public bool isActivated;
    public bool transformYCountAsSwitchActivation = true;
    public GameObject assiociatedObject;
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
    [Header("Load Components", order = 0)]
    [Space(10, order = 1)]
    public bool isLoading;
    bool checkMinRange = false;
    bool checkMinIntensity = false;
    bool checkMinYpos = false;
    public bool isAtMinimum = true;
    private bool haveSetAnEntry = false;
    public int nbEntryThisSwitchSet = 1;
    private Transform lightTransform;

    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        lightTransform = GameObject.Find("PlayerLight_v4-1").transform;
        thisObjectLight = GetComponent<Light>();
        minYPos = transform.position.y;
        minIntensity = thisObjectLight.intensity;
        minRange = thisObjectLight.range;
        mesh = GetComponent<MeshRenderer>();
        materials = mesh.materials;
        myMat = materials[1];
        maxYPos += transform.position.y;
        Invoke("ActivateAtStart", 0.1f);
    }
    private void Update()
    {
        if (!isActivated && isLoading)
        {
            if (playerLight.GetComponent<LightDetection>() != null)
            {
                if (Vector3.Distance(transform.position, playerLight.transform.position) > playerLight.GetComponent<LightDetection>().range * 2)
                {
                    deactivateEvent.Invoke();
                }
            }
        }
        if (Vector3.Distance(transform.position, lightTransform.position) < 5f && isActivated == false)
        {
            outline.SetOutline();
        }
        else
        {
            outline.RemoveOutline();
        }
    }

    public void Loading()
    {
        if (!isActivated)
        {
            isAtMinimum = false;
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
            if (transform.position.y < maxYPos && transformYCountAsSwitchActivation)
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
            if (transformYCountAsSwitchActivation)
            {
                if (rangeIsMaxed && transformIsMaxed && intensityIsMaxed)
                {
                    Activation();
                }
            }
            else
            {
                if (rangeIsMaxed && intensityIsMaxed)
                {
                    Activation();
                }
            }
        }
    }

    public void Unload()
    {
        if (thisObjectLight.intensity > minIntensity)
        {
            thisObjectLight.intensity -= lightGrowFactor * 4;
        }
        else
        {
            checkMinIntensity = true;
        }
        if (transform.position.y > minYPos)
        {
            transform.Translate(Vector3.down * transformMoveFactor * 2);
        }
        else
        {
            checkMinYpos = true;
        }
        if (thisObjectLight.range > minRange)
        {
            thisObjectLight.range -= rangeGrowFactor * 4;
        }
        else
        {
            checkMinRange = true;
        }
        if (checkMinIntensity && checkMinRange && checkMinYpos)
        {
            isAtMinimum = true;
        }
        Deactivate();
    }

    private void Deactivate()
    {
        myMat.DisableKeyword("_EMISSION");
        isActivated = false;
        deactivateEvent.Invoke();
        if (assiociatedObject != null)
        {
            if (assiociatedObject.GetComponent<MultipleEntryDoor>().ActualEntriesSet > 0 && haveSetAnEntry)
            {
                assiociatedObject.GetComponent<MultipleEntryDoor>().SetNewEntry(-nbEntryThisSwitchSet);
                haveSetAnEntry = false;
            }
        }
    }

    private void Activation()
    {
        CameraShake.Shake(0.05f, 0.2f);
        myMat.EnableKeyword("_EMISSION");
        isLoading = false;
        Instantiate(maxLightVfx, transform.position, Quaternion.identity);
        isActivated = true;
        activationEvent.Invoke();
        if (playerLight.GetComponent<LightDetection>() != null)
        {
           // playerLight.GetComponent<LightDetection>().StopFollow();
        }
        if (assiociatedObject != null)
        {
            if (!haveSetAnEntry)
            {
                assiociatedObject.GetComponent<MultipleEntryDoor>().SetNewEntry(nbEntryThisSwitchSet);
                haveSetAnEntry = true;
            }
        }
        GetComponent<ChainReaction>().GetSwitchInRange();
    }
    private void ActivateAtStart()
    {
        if (activateAtStart == true)
        {
            playerLight = FindObjectOfType<LightManager>().gameObject;

            transform.position = transform.position + transform.up;
            thisObjectLight.intensity = maxIntensity;
            thisObjectLight.range = maxRange;
            Activation();
        }
    }
}