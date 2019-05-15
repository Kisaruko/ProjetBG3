using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttackAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EndAttackEvent()
    {
        animator.SetBool("Attack", false);
    }
}
