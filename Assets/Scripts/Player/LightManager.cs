using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private PlayerMovement playermovement;
    private BinaryLight binarylight;
    private int frames = 0;
    private Rigidbody rb;
    public Light shortLight;
    public float maxIntensity;
    public float minIntensity;
    public float increaseFactor;
    public float decreaseFactor;
    public float dashDecreaseMultiplier;
    void Start()
    {
        playermovement = GetComponentInParent<PlayerMovement>();
        binarylight = GetComponentInParent<BinaryLight>();
        rb = GetComponent<Rigidbody>();
        shortLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        frames++;
        if(!playermovement.isMoving && frames % 10 == 0 && binarylight.gotLight)
        {
            LightIncreasing();
        }
        if(playermovement.isMoving && frames % 10 == 0 && binarylight.gotLight)
        {
            LightDecreasing();
        }
        if (playermovement.isDashing && frames % 2 ==0 && binarylight.gotLight)
        {
            DashDecrease();
        }
        if (binarylight.isRegrabable && frames % 30 == 0 && !binarylight.gotLight)
        {
            LightIncreasing();
        }

    }
    public void LightIncreasing()
    {
        if(shortLight.intensity < maxIntensity)
        {
            shortLight.intensity += increaseFactor;
        }
    }
    public void LightDecreasing()
    {
        if (shortLight.intensity > minIntensity)
        {
            shortLight.intensity -= decreaseFactor;
        }
    }
    public void DashDecrease()
    {
        if (shortLight.intensity > minIntensity)
        {
            shortLight.intensity -= decreaseFactor *dashDecreaseMultiplier;
        }
    }

}
