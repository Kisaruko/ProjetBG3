using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    private Transform playerTransform;
    private Transform playerLightTransform;
    [SerializeField]private float detectionRange;
    private NavMeshAgent meshAgent;

    private void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").transform;
        playerLightTransform = GameObject.Find("PlayerLight_v4-1").transform;
    }

    private void Update()
    {
        if(!DetectPlayer())
        {
            DetectPlayer();
        }

        if(DetectPlayer())
        {
            FollowLight();
        }
    }

    private bool DetectPlayer()
    {
        if(Vector3.Distance(this.transform.position, playerTransform.position) <= detectionRange)
        {
            //Debug.Log("Player detected");
            return true;
        }
        else
        {
            //Debug.Log("Player is not in range");
            return false;
        }
    }

    private void FollowLight()
    {
        meshAgent.destination = playerLightTransform.position;
    }
}
