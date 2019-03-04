using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int lifePoints;
    private int currentLifePoints;
    public GameObject fxDeath;
    public AudioClip deathSound;
    public AudioClip isHit;
    public GameObject[] possibleRewards;
    public int chanceToReward;
    private int isRewarding = 0;
    private int whichReward;
    public GameObject[] otherSpawn;

    private void Start()
    {
        currentLifePoints = lifePoints;
    }
    public void LostLifePoint(int damageDeal)
    {
        currentLifePoints = currentLifePoints -damageDeal;
        //AudioSource.PlayClipAtPoint(isHit, Camera.main.transform.position);
        if(currentLifePoints <= 0)
        {
            //RandomReward();
            Death();
        }
    }
    void Death()
    {
        //AudioSource.PlayClipAtPoint(deathSound,Camera.main.transform.position);
        Instantiate(fxDeath, transform.position, Quaternion.identity);
        //InstantiateOthers();
        Destroy(this.gameObject);
    }
    void RandomReward()
    {
        chanceToReward = Random.Range(isRewarding, chanceToReward);
        if(chanceToReward == isRewarding)
        {
            whichReward = Random.Range(0, possibleRewards.Length);
            Instantiate(possibleRewards[whichReward], transform.position, transform.rotation);
        }
    }
    void InstantiateOthers()
    {
        if(otherSpawn.Length >0)
        {
            Vector2 spawnPos = this.transform.position;
            float range =0.5f;
            for (int i = 0; i < otherSpawn.Length; i++)
            {
                spawnPos = spawnPos + new Vector2(Random.Range(-range,range ), (Random.Range(-range,range)));
                Instantiate(otherSpawn[1], spawnPos, Quaternion.identity);
            }                
        }
    }
        
}
