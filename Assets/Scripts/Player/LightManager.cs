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
    public float decreaseFactor;
    public float dashDecreaseMultiplier;
    public bool canDash;
    public GameObject fxMaxLight;
    private bool fxMaxLightHasBeenInstantiate;


    void Start()
    {
        playermovement = GetComponentInParent<PlayerMovement>();
        binarylight = GetComponentInParent<BinaryLight>();
        rb = GetComponent<Rigidbody>();
        shortLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (shortLight.intensity >= maxIntensity)
        {
            canDash = true;
        }
        if (!playermovement.isMoving && binarylight.gotLight)
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
    }

    IEnumerator StartIncrease()
    {
        yield return new WaitForSecondsRealtime(increaseTime);
        LightIncreasing();
    }
    IEnumerator StartDecrease()
    {
        yield return new WaitForSecondsRealtime(decreaseTime);
        LightDecreasing();
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

    public void LightDecreasing()
    {
        if (shortLight.intensity > minIntensity)
        {
            shortLight.intensity -= decreaseFactor;
            fxMaxLightHasBeenInstantiate = false;
        }
    }
}
