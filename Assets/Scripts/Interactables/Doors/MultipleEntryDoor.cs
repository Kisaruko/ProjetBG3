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
    public bool reverseBehaviour;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if(reverseBehaviour)
        {
            if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
            {
                transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Activate();
            }
            else
            {
                Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
            }
            Activate();
        }
    }
    public void SetNewEntry(int nb)
    {
        ActualEntriesSet += nb;
        CheckIfAllEntriesAreSet();
    }
    private void CheckIfAllEntriesAreSet()
    {
        if (!reverseBehaviour)
        {
            if (ActualEntriesSet >= NumberOfEntries)
            {
                if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
                {
                    transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Activate();
                }
                else
                {
                    Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
                }
                Activate();
            }
            else
            {
                if (canBeDesactivated)
                {
                    if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
                    {
                        transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Deactivate();
                    }
                    else
                    {
                        Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
                    }
                    Deactivate();
                }
            }
        }
        else
        {
            if (ActualEntriesSet >= NumberOfEntries)
            {
                if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
                {
                    transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Deactivate();
                }
                else
                {
                    Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
                }
                Deactivate();
            }
            else
            {
                if (canBeDesactivated)
                {
                    if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
                    {
                        transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Activate();
                    }
                    else
                    {
                        Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
                    }
                    Activate();
                }
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
