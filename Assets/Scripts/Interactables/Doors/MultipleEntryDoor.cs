using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleEntryDoor : ActivableObjects
{
    public int NumberOfEntries;
    public int ActualEntriesSet;
    private Animator anim;
    public bool canBeDesactivated;
    public int nbChildThatGotEmissive = 2;
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
            if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
            {
                transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Activate();
            }
            else
            {
                Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
            }
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
