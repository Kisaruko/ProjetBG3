using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingTreeBehaviour : ActivableObjects
{
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
    public float timeBeforeResetCam;
    private Light thisObjectLight;
    private bool increaseRange;
    private Material[] myMats;
    private Material troncMat;
    private Material receptacleMat;
    private Material[] socleMats;
    public GameObject ambiantFx;
    public GameObject highLightDark;
    [Header("Camera and time Variables")]
    public float TBeforeResetCamAndControls;
    public float TForCamToBeReset;


    private void Start()
    {
        thisObjectLight = GetComponentInChildren<Light>();
        myMats = GetComponentInChildren<MeshRenderer>().materials;
        troncMat = transform.GetChild(1).GetComponent<MeshRenderer>().material;
        receptacleMat = transform.GetChild(4).GetComponent<MeshRenderer>().material;
        socleMats = transform.GetChild(5).GetComponent<MeshRenderer>().materials;
    }
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public void Loading()
    {
        if (!isActivated)
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
                EnlightTree();
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
    private void EnlightTree()
    {
        if(ambiantFx !=  null)
        {
            ambiantFx.SetActive(true);
            highLightDark.SetActive(false);
        }
        FindObjectOfType<PlayerMovement>().DisableControls(transform);
        FindObjectOfType<BinaryLight>().DisableControls();
        FindObjectOfType<LightDetection>().DisableControls();

        Camera.main.GetComponentInParent<CameraBehaviour>().smoothSpeed = 0.1f;

        Camera.main.GetComponentInParent<CameraBehaviour>().SetNewParameters(35, 25, 45, 2f);

        Camera.main.GetComponentInParent<CameraBehaviour>().target = thisObjectLight.transform;
        CameraShake.Shake(0.05f, 0.2f);

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
            receptacleMat.SetColor("_EmissionColor", Color.white*2);
        }
        isLoading = false;
        Instantiate(activationFx, transform.position, Quaternion.identity);
        isActivated = true;
        increaseRange = true;
        StartCoroutine("ResetCam");
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
        if(other.GetComponent<TrashMobManager>() != null)
        {
            Instantiate(destroyMobFx, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        if(other.GetComponent<PressurePlateBehaviour>() != null)
        {
            other.GetComponent<PressurePlateBehaviour>().nbObjectOnThis = 10;
            other.GetComponent<PressurePlateBehaviour>().SetObjectOnThis();
        }
        /*if(other.GetComponent<SpawnerOneByOne>() != null) //Detruit les spawner si besoin (doivent avoir une collider)
        {
            Destroy(other.gameObject);
        }*/
        if(other.GetComponent<CorruptionBehaviour>() != null)
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
        if(range >= sphereMaxRange)
        {
            if(GetComponent<SphereCollider>() != null)
            {
                GetComponent<SphereCollider>().enabled = false;
            }
        }
    }
    private IEnumerator ResetCam()
    {
        Camera.main.GetComponentInParent<CameraBehaviour>().target = FindObjectOfType<PlayerMovement>().transform;
        yield return new WaitForSeconds(TBeforeResetCamAndControls);
        Camera.main.GetComponentInParent<CameraBehaviour>().ResetCamParameters(TForCamToBeReset);
        Instantiate(ambiantGoodVfx, transform.position, Quaternion.identity);
        FindObjectOfType<PlayerMovement>().EnableControls();
        FindObjectOfType<BinaryLight>().EnableControls();
        FindObjectOfType<LightDetection>().EnableControls();
        StopCoroutine("ResetCam");

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sphereMaxRange);
    }
}
