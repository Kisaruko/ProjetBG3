using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArthropodeBehaviour : MonoBehaviour
{
    private Animator anim;
    private Vector3 InitPos;
    private Vector3 newPos;


    public float speed;
    public float range;
    private Vector3 vectorRange;
    public bool isMoving;
    public float timeOnIdle;
    private bool mustMove;
    bool hasStartedIdle = false;

    bool havechooseDestination;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        InitPos = transform.position;
        SelectDestination();
    }

    private void Update()
    {
        DecisionMaker();
        if (mustMove == false)
        {
            transform.DOLookAt(newPos, 0.5f);
            anim.SetBool("isMoving", false);
        }
    }
    void SelectDestination()
    {
        anim.SetBool("isMoving", false);
        vectorRange = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
        newPos = (InitPos + vectorRange);
    }
    void DecisionMaker()
    {
        if (Vector3.Distance(transform.position, newPos) < 0.1f)
        {
            if (havechooseDestination == false)
            {
                mustMove = false;
                Idle();
                SelectDestination();
                havechooseDestination = true;
            }
            anim.SetBool("isMoving", false);
        }
        else
        {
            mustMove = true;
            Movement();
        }
    }
    void Movement()
    {
        if (mustMove)
        {
            isMoving = true;
            anim.SetBool("isMoving", true);
            transform.DOMove(newPos, speed);
            hasStartedIdle = false;
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
    void Idle()
    {
        if (!mustMove)
        {
            if (!hasStartedIdle)
            {
                isMoving = false;
                anim.SetBool("isMoving", false);
                hasStartedIdle = true;
                Invoke("CoolDownBeforeMove", timeOnIdle);
            }
            anim.SetBool("isMoving", false);
        }
    }
    private void CoolDownBeforeMove()
    {
        mustMove = true;
        havechooseDestination = false;
    }
}
