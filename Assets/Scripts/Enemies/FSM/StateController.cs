using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public TrashMobStats trashMobStats;
    public Transform eyes;
    public State remainState;
    public GameObject fxHit;
    public Animator animator;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed = 0f;

    private bool aiActive;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //Get the Nav Mesh Agent Component
    }

    public void SetupAI(bool aiActivationFromTrashMobManager, List<Transform> wayPointsFromTrashMobManager)
    {
        //Setup the AI, the way points list will be the one assigned when calling this method, same for the activation of the AI
        wayPointList = wayPointsFromTrashMobManager;
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

    private void OnDrawGizmos()
    {
        if(currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, trashMobStats.lookSphereCastRadius);
        }
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
}
