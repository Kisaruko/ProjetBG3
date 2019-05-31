using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class ArianeBondBehaviour : MonoBehaviour
{
    public Transform destination;
    private GameObject lightAnchor;
    private GameObject lightObject;
    public float vfxSpeed;
    public float rangeBeforeComeBack;

    private NavMeshAgent agent;

    private void Start()
    {
        lightObject = GameObject.Find("PlayerLight_v4-1");
        lightAnchor = GameObject.Find("PlayerLight");
        agent = GetComponent<NavMeshAgent>();
        destination = lightObject.transform;
    }
    private void Update()
    {
        if(agent.enabled && !agent.isOnNavMesh)
        {
            Vector3 position = transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 10.0f, -1);
            position = hit.position;
            agent.Warp(position);
        }

        if(destination != null)
        {
            if(destination == lightAnchor.transform)
            {
                agent.SetDestination(lightAnchor.transform.position);
                if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    destination = lightObject.transform;
                }
            }
            if(destination == lightObject.transform)
            {
                agent.SetDestination(lightObject.transform.position);
                if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    destination = lightAnchor.transform;
                }
            }
        }

        if(agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Vector3 position = transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 10.0f, -1);
            position = hit.position;
            agent.Warp(position);
        }
    }

}
