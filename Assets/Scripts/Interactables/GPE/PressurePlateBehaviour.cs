﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateBehaviour : MonoBehaviour
{
    [Header("Activation Variables")]
    public bool isStartingActive;
    public List<string> activationTag;
    public bool isActivated;
    public GameObject[] associatedGameobjects;
    public UnityEvent activateEvent = new UnityEvent();
    public UnityEvent deactivateEvent = new UnityEvent();

    public GameObject multipleEntryDoor;

    public int nbObjectOnThis = 0;

    [Header("Pressure Plate Variables")]
    public float pressureFactorY;
    private float startingPosY;

    [Header("Emission Variables")]
    public Color baseColor;
    private Material material;
    private Color finalColor;
    private float emission;
    private bool haveSetAnEntry;
    void Start()
    {
        material = GetComponentInChildren<Renderer>().material; //Get the material in order to change its Emissive color
        startingPosY = transform.position.y; //Get the position Y in order to change it for when it has pressure on it
        Invoke("SetDoorAtBeginning", 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Compare if there is a tag in the List of tag
        foreach (string taggedTrigger in activationTag)
        {
            if (other.CompareTag(taggedTrigger))
            {
                //The pressure plate is activated and its Y pos is changed with the pressureFactorY
                transform.position = new Vector3(this.transform.position.x, transform.position.y - pressureFactorY, this.transform.position.z);
                isActivated = true;
                /*for (int i = 0; i < associatedGameobject.Length; i++)
                {
                    associatedGameobject[i].GetComponent<ActivableDoorBehaviour>().Activate();
                }*/
                nbObjectOnThis += 1;
                ExecuteAnimation();
            }
        }
        if(multipleEntryDoor != null)
        {
            if (!haveSetAnEntry)
            {
                multipleEntryDoor.GetComponent<MultipleEntryDoor>().SetNewEntry(1);
                haveSetAnEntry = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Compare if there is a tag in the List of tag
        foreach (string taggedTrigger in activationTag)
        {
            if (other.CompareTag(taggedTrigger))
            {
                //The pressure plate is not activated || its Y pos comes back to normal || Reset the emission color
                transform.position = new Vector3(this.transform.position.x, transform.position.y + pressureFactorY, this.transform.position.z);
                isActivated = false;
                material.SetColor("_EmissionColor", new Color(0, 0, 0));
                /*for (int i = 0; i < associatedGameobject.Length; i++)
                {
                    associatedGameobject[i].GetComponent<ActivableDoorBehaviour>().Deactivate();
                }*/
                nbObjectOnThis -= 1;
                ExecuteAnimation();
            }
        }
        if (multipleEntryDoor != null)
        {
            if (multipleEntryDoor.GetComponent<MultipleEntryDoor>().ActualEntriesSet > 0 && haveSetAnEntry)
            {
                multipleEntryDoor.GetComponent<MultipleEntryDoor>().SetNewEntry(-1);
                haveSetAnEntry = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Compare if there is a tag in the List of tag
        foreach (string taggedTrigger in activationTag)
        {
            if (other.CompareTag(taggedTrigger))
            {
                emission = Mathf.PingPong(Time.time, 1.0f); //float qui fait un pingpong entre le temps actuel et 1.0f
                finalColor = baseColor * Mathf.LinearToGammaSpace(emission); //Applique la couleur de l'emissive fois l'intensité du pingpong

                material.SetColor("_EmissionColor", finalColor); //Applique le pingpong de l'emissive sur le material
            }
        }
    }
    private void SetDoorAtBeginning()
    {
        if (isStartingActive)
        {
            activateEvent.Invoke();
        }
        else
        {
            deactivateEvent.Invoke();
        }
    }
    private void ExecuteAnimation()
    {
        if (nbObjectOnThis > 0)
        {
            activateEvent.Invoke();
        }
        if (nbObjectOnThis <= 0)
        {
            deactivateEvent.Invoke();
        }
    }
}
