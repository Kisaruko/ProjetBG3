using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Gate { Red = 4, Green, Blue }

public class StateController : MonoBehaviour
{
    public State currentState;
    public TrashMobStats trashMobStats;
    public Transform eyes;
    public State remainState;
    public GameObject fxHit;
    public Animator animator;
    public GameObject fxAbsorb;

    [HideInInspector] public Vector3 spawnPosition;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed = 0f;

    private bool aiActive;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //Get the Nav Mesh Agent Component
        spawnPosition = transform.position;
    }

    public void SetupAI(bool aiActivationFromTrashMobManager)
    {
        SetGateAllowed(Gate.Green, true);
        //Setup the AI, the way points list will be the one assigned when calling this method, same for the activation of the AI
        aiActive = aiActivationFromTrashMobManager;
        if(aiActive)
        {
            navMeshAgent.enabled = true; //Active the nav mesh agent
        }
        else
        {
            navMeshAgent.enabled = false; //Disable the nav mesh agent
        }
    }

    private void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
        stateTimeElapsed += Time.deltaTime;
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState) //If the next state is different from the remain state it goes to the next state
        {
            currentState = nextState;
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        return (stateTimeElapsed >= duration);
    }

    private void SetGateAllowed(Gate gate, bool allowed)
    {
        if (allowed)
        {
            navMeshAgent.areaMask |= 1 << (int)gate;
        }
        else
            navMeshAgent.areaMask &= ~(1 << (int)gate);
    }
}
