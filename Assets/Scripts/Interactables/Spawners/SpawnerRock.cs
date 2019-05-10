using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRock : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeBeforeSpawn;
    public float spawnRate;
    public bool isSpawning = true;

    void Start()
    {
        InvokeRepeating("SpawnObject", timeBeforeSpawn, spawnRate);
    }
    public void SpawnObject()
    {
        if(isSpawning)
        {
          GameObject clone = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
          Destroy(clone, 10f);
        }
    }
}
