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
        GetComponent<FMODUnity.StudioEventEmitter>().SetParameter("doorActivated", 1f);
        SendMessage("Play");
    }
    public override void Deactivate()
    {
        animator.SetBool("isActivated", false);
        GetComponent<FMODUnity.StudioEventEmitter>().SetParameter("doorActivated", 0f);
        SendMessage("Play");
    }
    private void SetAtStart()
    {
        Activate();
    }
}
