using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Life Attributes", order = 0)]
    [Space(10, order = 1)]
    public float currentLife;
    public float maxLife;
    public float minLife;
    public bool isInvicible;
    public float invincibleDuration;
    public float lifeRegen;

    [Header("Life Minimum To Use Skills", order = 0)]
    public float minLifeToShoot;
    public float minLifeToBeam;
    public float minLifeToDash;


    [Header("Can Player Use Skills ?", order = 0)]
    [Space(10, order = 1)]
    public bool canShoot;
    public bool canBeam;
    public bool canDash;

    [Header("Light Attributes", order = 0)]
    public Light shortLight;
    public Light midLight;
    private float shortLightMaxIntensity;
    private float midLightMaxIntensity;
    private float shortLightRatio;
    private float midLightRatio;

    private void Start()
    {
        currentLife = maxLife;
        CheckIfPlayerCanUseSkills();

        CalculateLightRatioForOneHP();
        shortLightMaxIntensity = shortLight.intensity;
        midLightMaxIntensity = midLight.intensity;
    }

    #region Custom Methods

    #region Attack Life Methods
    public void RegenLifeOnCac()
    {
        if (currentLife < maxLife && currentLife > minLife)
        {
            currentLife += lifeRegen / LightMagnetism.nbParticles;
            if(shortLight.intensity <= shortLightMaxIntensity && midLight.intensity < midLightMaxIntensity)
            {
                shortLight.intensity += shortLightRatio * (lifeRegen / LightMagnetism.nbParticles);
                midLight.intensity += midLightRatio * (lifeRegen / LightMagnetism.nbParticles);
            }
            
            if (currentLife > maxLife)
            {
                currentLife = maxLife;
            }
            if (currentLife < minLife)
            {
                currentLife = minLife;
            }

        }
        CheckIfPlayerCanUseSkills();
    }
    #endregion

    #region Shoot Life Methods
    public void UseLifeOnShoot(float lifeUsageOnShoot)
    {
        CheckIfPlayerCanUseSkills();
        if (canShoot)
        {
            currentLife -= lifeUsageOnShoot;
            LightUsage(lifeUsageOnShoot);
            CheckIfPlayerCanUseSkills();
        }
    }
    #endregion

    #region Beam Life Methods
    public void UseLifeToLoadBeam(float lifeUsageToLoadBeam)
    {
        CheckIfPlayerCanUseSkills();
        if (canBeam)
        {
            currentLife -= lifeUsageToLoadBeam;
            LightUsage(lifeUsageToLoadBeam);
            CheckIfPlayerCanUseSkills();
        }
    }

    public void UseLifeEachInterval(float lifeUsageEachInterval)
    {
        CheckIfPlayerCanUseSkills();
        if (canBeam)
        {
            currentLife -= lifeUsageEachInterval;
            LightUsage(lifeUsageEachInterval);
            CheckIfPlayerCanUseSkills();
        }
    }
    #endregion

    #region Dash Life Methods
    public void UseLifeOnDash(float lifeUsageOnDash)
    {
        CheckIfPlayerCanUseSkills();
        if (canDash)
        {
            currentLife -= lifeUsageOnDash;
            LightUsage(lifeUsageOnDash);
            CheckIfPlayerCanUseSkills();
        }
    }
    #endregion

    #region Check The Player Life
    private void CheckIfPlayerCanUseSkills()
    {
        if (currentLife < minLifeToDash && currentLife > minLife)
        {
            canDash = false;
        }
        else
        {
            canDash = true;
        }

        if (currentLife < minLifeToShoot && currentLife > minLife)
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }

        if(currentLife < minLifeToBeam && currentLife > minLife)
        {
            canBeam = false;
        }
        else
        {
            canBeam = true;
        }
        
    }
    #endregion

    private void CalculateLightRatioForOneHP()
    {
        shortLightRatio = shortLight.intensity / maxLife ;
        midLightRatio = midLight.intensity / maxLife;
        Debug.Log("Short light ratio is : " + shortLightRatio + " | Mid light ratio is : " + midLightRatio);
    }

    private void LightUsage(float lifeUsage)
    {
        shortLight.intensity -= shortLightRatio * lifeUsage;
        midLight.intensity -= midLightRatio * lifeUsage;
    }

    #region Player Taking Damage
    public void TakeHit(int damage)
    {
        if (isInvicible == false)
        {
            currentLife -= damage;
            isInvicible = true;
            StartCoroutine("InvicibleTime");
        }
    }

    IEnumerator InvicibleTime()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvicible = false;
        StopCoroutine("InvincibleTime");
    }
    #endregion

    #endregion
}