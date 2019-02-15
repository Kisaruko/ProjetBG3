using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Life Attributes", order = 0)]
    [Space(10, order = 1)]
    public float currentLife;
    public float maxLife;
    public float minLife;

    [Header("Life Skills Attributes", order = 0)]
    [Space(10, order = 1)]
    public float maxLifeToShoot;
    public float lifeUsageOnShoot;
    [Space(5, order = 2)]
    public float maxLifeToDash;
    public float lifeUsageOnDash;
    [Space(5, order = 3)]
    public float lifeRegen;

    private bool canShoot;
    private bool canDash;

    void Update()
    {
        CheckIfPlayerCanUseSkills();

        //Regenerate player life
        if(Input.GetMouseButtonDown(0))
        {
            if(currentLife < maxLife && currentLife > minLife)
            {
                currentLife += lifeRegen;
            }
        }

        //Use X% of player maxLife
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(canShoot)
            {
                currentLife -= lifeUsageOnShoot;
            }
        }

        //Use X% of player maxLife
        if (Input.GetMouseButtonDown(1))
        {
            if(canDash)
            {
                currentLife -= lifeUsageOnDash;
            }
        }

        currentLife = Mathf.Round(currentLife);
    }

    private void CheckIfPlayerCanUseSkills()
    {
        if (currentLife < maxLifeToDash && currentLife > minLife)
        {
            canDash = false;
        }
        else
            canDash = true;
        if (currentLife < maxLifeToShoot && currentLife > minLife)
        {
            canShoot = false;
        }
        else
            canShoot = true;
    }
}