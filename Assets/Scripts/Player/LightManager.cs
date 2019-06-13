using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private PlayerMovement playermovement;
    private BinaryLight binarylight;
    private Rigidbody rb;
    public Light shortLight;
    public float maxIntensity;
    public float minIntensity;
    public float increaseTime;
    public float decreaseTime;
    public float increaseFactor;
    public float dashDecreaseFactor;
    public float dashDecreaseMultiplier;
    public bool canDash;
    public GameObject fxMaxLight;
    private bool fxMaxLightHasBeenInstantiate;


    void Start()
    {
        playermovement = FindObjectOfType<PlayerMovement>();
        binarylight = FindObjectOfType<BinaryLight>();
        rb = GetComponent<Rigidbody>();
        shortLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (shortLight.intensity >= maxIntensity)
        {
            canDash = true;
        }
        if (binarylight.gotLight)
        {
            StartCoroutine("StartIncrease");
        }
        if (playermovement.isDashing && binarylight.gotLight)
        {
            StartCoroutine("StartDecrease");
        }
        if (binarylight.isRegrabable && !binarylight.gotLight)
        {
            StartCoroutine("StartIncrease");
        }
        if(shortLight.intensity <= 0.005f)
        {
            GameManager._instance.GameOver();
        }
    }

    IEnumerator StartIncrease()
    {
        yield return new WaitForSecondsRealtime(increaseTime);
        LightIncreasing();
    }
    IEnumerator StartDecrease()
    {
        yield return new WaitForSecondsRealtime(decreaseTime);
        LightDecreasing(dashDecreaseFactor);
    }
    public void LightIncreasing()
    {
        if(shortLight.intensity < maxIntensity)
        {
            shortLight.intensity += increaseFactor;
        }
        else
        {
             if(!fxMaxLightHasBeenInstantiate)
            {
                Instantiate(fxMaxLight, transform.position, transform.rotation);
                fxMaxLightHasBeenInstantiate = true;
            }
        }
    }

    public void LightDecreasing(float decreaseFactor)
    {
        if (shortLight.intensity > minIntensity)
        {
            shortLight.intensity -= decreaseFactor;
            fxMaxLightHasBeenInstantiate = false;
        }
    }
}
