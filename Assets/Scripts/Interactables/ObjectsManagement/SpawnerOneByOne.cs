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
    public bool mustLockXRigidbody;
    public float timeBeforeDestroy;
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
            if (mustLockXRigidbody)
            {
                clone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            }
            //SecurityCheck
            if (destroyByTime)
            {
                Destroy(clone, timeBeforeDestroy);
            }
        }
    }
    private void OnDestroy()
    {
        if(clone != null)
        {
            Destroy(clone.gameObject);
        }
    }
}
