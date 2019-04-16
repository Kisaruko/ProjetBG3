using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/TrashMobStats")]
public class TrashMobStats : ScriptableObject
{
    public float moveSpeed;
    public float lookRange;
    public float lookSphereCastRadius;

    public float attackRange;
    public float attackRate;
    public float attackForce;
    public int attackDamage;
    public float attackSphereCastRadius;
}
