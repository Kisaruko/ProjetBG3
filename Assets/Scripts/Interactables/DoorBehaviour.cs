using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : ActivableObjects
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        animator.SetBool("isActivated", true);
    }
    public override void Deactivate()
    {
        animator.SetBool("isActivated", true);
    }
}
