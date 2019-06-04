using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOneByOne : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float checkTimeBeforeEachInvoke;
    private GameObject clone;


    private void Start()
    {
        InvokeRepeating("SpawnObject", 1f, checkTimeBeforeEachInvoke);
    }
    private void SpawnObject()
    {
        if(clone == null)
        {
            clone = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            //SecurityCheck
            Destroy(clone, 100);
        }
    }
}
