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

    [Header("Trappe Only")]
    public bool isAtrap;
    public GameObject[] roots;
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
                if (!isAtrap)
                {
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
                    for (int i = 0; i < roots.Length; i++)
                    {
                        roots[i].GetComponent<RootBehaviour>().Activate();
                    }
                }
                Activate();

            }
            else
            {
                if (canBeDesactivated)
                {
                    if (!isAtrap)
                    {
                        if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
                        {
                            transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Deactivate();
                        }
                        else
                        {
                            Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < roots.Length; i++)
                        {
                            roots[i].GetComponent<RootBehaviour>().Deactivate();
                        }
                    }
                    Deactivate();
                }
            }
        }
        else
        {
            if (ActualEntriesSet >= NumberOfEntries)
            {
                if (!isAtrap)
                {
                    if (transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>() != null)
                    {
                        transform.GetChild(nbChildThatGotEmissive).GetComponent<RootBehaviour>().Deactivate();
                    }
                    else
                    {
                        Debug.LogWarning("This child (number in script) doesn't got a rootBehaviour");
                    }
                }
                for (int i = 0; i < roots.Length; i++)
                {
                    roots[i].GetComponent<RootBehaviour>().Activate();
                }
                Deactivate();
            }
            else
            {
                if (canBeDesactivated)
                {
                    if (!isAtrap)
                    {
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
                        for (int i = 0; i < roots.Length; i++)
                        {
                            roots[i].GetComponent<RootBehaviour>().Deactivate();
                        }
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
