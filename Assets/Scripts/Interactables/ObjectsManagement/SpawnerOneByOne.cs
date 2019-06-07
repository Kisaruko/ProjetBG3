using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOneByOne : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float checkTimeBeforeEachInvoke;
    private GameObject clone;
    public bool destroyByTime;
    public bool mustLockZRigidbody;

    private void Start()
    {
        InvokeRepeating("SpawnObject", 1f, checkTimeBeforeEachInvoke);
    }
    private void SpawnObject()
    {
        if(clone == null)
        {
            clone = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            if (mustLockZRigidbody)
            {
                clone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            }
            //SecurityCheck
            if (destroyByTime)
            {
                Destroy(clone, 100);
            }
        }
    }
}
