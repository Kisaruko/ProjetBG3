using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMobManager : MonoBehaviour
{
    public bool aiActive;
    private StateController controller;

    private void Awake()
    {
        controller = GetComponent<StateController>();
        aiActive = true;
    }

    private void Start()
    {
        controller.SetupAI(aiActive);
    }
}
