using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : ActivableObjects
{
    private Animator animator;
    public bool activateAtStart;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (activateAtStart)
        {
            Invoke("SetAtStart", 1f);
        }
    }

    public override void Activate()
    {
        animator.SetBool("isActivated", true);
    }
    public override void Deactivate()
    {
        animator.SetBool("isActivated", false);
    }
    private void SetAtStart()
    {
        Activate();
    }
}
