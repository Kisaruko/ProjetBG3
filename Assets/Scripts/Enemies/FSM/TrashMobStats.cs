﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/TrashMobStats")]
public class TrashMobStats : ScriptableObject
{
    [Header("Movement Variables")]
    public float moveSpeed;
    public float rotationSpeed;

    [Header("Patrol Variables")]
    public float patrolDist;
    public float patrolWanderTime;

    [Header("Wandering Variables")]
    public float wanderTimeBeforePatrol;

    [Header("Detection Variables")]
    public float lookRange;
    [Range(0,360)]
    public float lookAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    //[HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    [Header("Attack Variables")]
    public float attackRange;
    public float attackCooldown;

    [Header("Absorb Variables")]
    public float absorbRange;
    public float absorbCooldown;
    public float absorbFactor;

    [Header("Disable Lights Variables")]
    public float disableRange;
    public LayerMask objectsToDisable;
    public float disableCooldown;

    [Header("Materials Variables")]
    public Material myMat;
    public Color baseColor;
    public Color finalColor;


}
