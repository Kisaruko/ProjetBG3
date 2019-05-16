using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleEntryDoor : ActivableObjects
{
    public int NumberOfEntries;
    public int ActualEntriesSet;
    private Animator anim;
    public bool canBeDesactivated;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void SetNewEntry(int nb)
    {
        ActualEntriesSet += nb;
        CheckIfAllEntriesAreSet();
    }

    private void CheckIfAllEntriesAreSet()
    {
        if(ActualEntriesSet >= NumberOfEntries)
        {
            Activate();
        }
        else
        {
            if (canBeDesactivated)
            {
                Deactivate();
            }
        }
    }
    public override void Activate()
    {
        anim.SetBool("isActivated", true);
    }
    public override void Deactivate()
    {
        anim.SetBool("isActivated", false);
    }
}
