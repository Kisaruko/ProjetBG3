using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralTreeBehaviour : MonoBehaviour
{
    public int nbEntrySet;
    public int nbEntryToBeSet = 2;
    public Transform newCamPos;

    /* [Header("GlowVariables", order = 0)]
     [Space(10, order = 1)]

     public bool startGlowing;
     public float minIntensity;
     public float maxIntensity;
     private Color baseColor;*/

    [Header("Activation Variables")]
    public bool isActivated;
    public bool isLoading;
    public float maxIntensity;
    public float maxRange;
    public float lightGrowFactor;
    public float rangeGlowFactor;
    private bool rangeIsMaxed;
    private bool intensityIsMaxed;

    [Header("Growing Variables")]
    public float range;
    public float rangeIncreaseFactor;
    public float sphereMaxRange;

    [Header("Visuals Variables")]

    public Transform lightTargetSpot;
    public GameObject activationFx;
    public GameObject destroyMobFx;
    public GameObject ambiantGoodVfx;
    private Light thisObjectLight;
    private bool increaseRange;
    private Material[] myMats;
    private Material troncMat;
    private Material receptacleMat;
    private Material[] socleMats;
    public GameObject ambiantFx;
    public GameObject highLightDark;
    private CentralTreeBehaviour centralTreeBehaviour;

    private void Start()
    {
        centralTreeBehaviour = FindObjectOfType<CentralTreeBehaviour>();
        thisObjectLight = GetComponentInChildren<Light>();
        myMats = GetComponentInChildren<MeshRenderer>().materials;
        troncMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
        receptacleMat = transform.GetChild(4).GetComponent<MeshRenderer>().material;
        socleMats = transform.GetChild(5).GetComponent<MeshRenderer>().materials;
    }

    public void CheckIfAllEntriesAreSet()
    {
        nbEntrySet++;
        if (nbEntrySet == 1)
        {

        }
        if (nbEntrySet >= nbEntryToBeSet)
        {
            highLightDark.SetActive(false);
            //DoSomething
        }
    }

    public void Loading()
    {
        if (!isActivated)
        {
            if (nbEntrySet >= 2)
            {
                isLoading = true;

                if (thisObjectLight.intensity < maxIntensity)
                {
                    thisObjectLight.intensity += lightGrowFactor;
                }
                else
                {
                    intensityIsMaxed = true;
                }
                if (thisObjectLight.range < maxRange)
                {
                    thisObjectLight.range += rangeGlowFactor;
                }
                else
                {
                    rangeIsMaxed = true;
                }
                if (rangeIsMaxed && intensityIsMaxed)
                {
                    isActivated = true;
                    GlobalIlluminationOfTree();
                }
            }
        }
    }
    #region glowBehaviour
    /*
    private void Start()
    {
        myMats = GetComponentInChildren<MeshRenderer>().materials;
        troncMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
        baseColor = myMats[1].color;
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
    }*/
    #endregion
    private void GlobalIlluminationOfTree()
    {
        if (ambiantFx != null)
        {
            ambiantFx.SetActive(true);
        }

        if (myMats[1] != null)
        {
            myMats[1].EnableKeyword("_EMISSION");
        }
        if (myMats[0] != null)
        {
            myMats[0].EnableKeyword("_EMISSION");
        }
        if (troncMat != null)
        {
            troncMat.EnableKeyword("_EMISSION");
        }
        if (socleMats[1] != null)
        {
            socleMats[1].EnableKeyword("_EMISSION");
            socleMats[1].SetColor("_EmissionColor", Color.white * 2);
        }
        if (troncMat != null)
        {
            receptacleMat.EnableKeyword("_EMISSION");
            receptacleMat.SetColor("_EmissionColor", Color.white * 2);
        }
        isLoading = false;
        Instantiate(activationFx, transform.position, Quaternion.identity);
        isActivated = true;
        increaseRange = true;
        if (GetComponent<RippleSpawn>() != null)
        {
            GetComponent<RippleSpawn>().SpawnRippleAtPoint(lightTargetSpot);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SwitchBehaviour>() != null && other.GetComponent<SwitchBehaviour>().isActivated == false)
        {
            other.GetComponent<SwitchBehaviour>().Activation();
        }
        if (other.GetComponent<EmitWhenTrigger>() != null)
        {
            other.GetComponent<EmitWhenTrigger>().ActivateEmission();
        }
        if (other.GetComponent<TrashMobManager>() != null)
        {
            Instantiate(destroyMobFx, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        if (other.GetComponent<PressurePlateBehaviour>() != null)
        {
            other.GetComponent<PressurePlateBehaviour>().nbObjectOnThis = 10;
            other.GetComponent<PressurePlateBehaviour>().SetObjectOnThis();
        }

        /*if(other.GetComponent<SpawnerOneByOne>() != null) //Detruit les spawner si besoin (doivent avoir une collider)
        {
            Destroy(other.gameObject);
        }*/

        if (other.GetComponent<CorruptionBehaviour>() != null)
        {
            other.GetComponent<CorruptionBehaviour>().Purification();
        }
    }
    private void Update()
    {
        if (increaseRange == true && range <= sphereMaxRange)
        {
            GetComponent<SphereCollider>().radius = range;
            thisObjectLight.range = range;
            range += rangeIncreaseFactor;
        }
        if (range >= sphereMaxRange)
        {
            if (GetComponent<SphereCollider>() != null)
            {
                GetComponent<SphereCollider>().enabled = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sphereMaxRange);
    }

    public void ShowTree(float speed)
    {
        Camera.main.GetComponentInParent<CameraBehaviour>().smoothSpeed = speed;
        Camera.main.GetComponentInParent<CameraBehaviour>().target = transform;
        Invoke("ResetCam", 6f);
    }
    private void ResetCam()
    {
        Camera.main.GetComponentInParent<CameraBehaviour>().target = FindObjectOfType<PlayerMovement>().transform;
        Camera.main.GetComponentInParent<CameraBehaviour>().ResetCamParameters(2f);
    }

}
