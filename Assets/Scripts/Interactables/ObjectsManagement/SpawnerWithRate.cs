using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithRate : ActivableObjects
{
    public GameObject objectToSpawn;
    public float timeBeforeSpawn;
    public float spawnRate;
    public bool isSpawning = true;
    public float timeBeforeDestroyObject;
    public int nbObjectToSpawnEachRate;

    public override void Activate()
    {
        InvokeRepeating("SpawnObject", timeBeforeSpawn, spawnRate);
    }
    public override void Deactivate()
    {
        CancelInvoke();
    }
    void Start()
    {
        if (isSpawning)
        {
            InvokeRepeating("SpawnObject", timeBeforeSpawn, spawnRate);
        }
    }
    public void SpawnObject()
    {
        if (isSpawning)
        {
            for (int i = 0; i < nbObjectToSpawnEachRate; i++)
            {
                GameObject clone = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                Destroy(clone, timeBeforeDestroyObject);
            }
        }
    }
}
