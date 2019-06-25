using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateBehaviour : MonoBehaviour
{
    [Header("Activation Variables")]
    public bool isStartingActive;
    public List<string> activationTag;
    public bool isActivated;
    public UnityEvent activateEvent = new UnityEvent();
    public UnityEvent deactivateEvent = new UnityEvent();

    public GameObject[] multipleEntryDoor;

    public int nbObjectOnThis = 0;

    [Header("Emission Variables")]
    public MeshRenderer emissiveMesh;
    private Material emissiveMat;
    private bool haveSetAnEntry;

    [Header("Vfx Variables")]
    public GameObject activationVFX;

    void Start()
    {
        emissiveMat = emissiveMesh.material;
        Invoke("SetDoorAtBeginning", 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Compare if there is a tag in the List of tag
        foreach (string taggedTrigger in activationTag)
        {
            if (other.CompareTag(taggedTrigger))
            {
                SetObjectOnThis();
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
                RemoveObjectOnThis();
            }
        }

    }

    public void SetObjectOnThis()
    {
        //The pressure plate is activated and its Y pos is changed with the pressureFactorY
        //transform.position = new Vector3(this.transform.position.x, transform.position.y - pressureFactorY, this.transform.position.z);
        isActivated = true;
        nbObjectOnThis += 1;
        ExecuteAnimation();
        if (multipleEntryDoor != null)
        {
            if (!haveSetAnEntry)
            {
                for (int i = 0; i < multipleEntryDoor.Length; i++)
                {
                    multipleEntryDoor[i].GetComponent<MultipleEntryDoor>().SetNewEntry(1);
                }
                haveSetAnEntry = true;
            }
        }
    }
    public void RemoveObjectOnThis()
    {
        //The pressure plate is not activated || its Y pos comes back to normal || Reset the emission color
        //transform.position = new Vector3(this.transform.position.x, transform.position.y + pressureFactorY, this.transform.position.z);
        isActivated = false;

        nbObjectOnThis -= 1;
        ExecuteAnimation();
        if (multipleEntryDoor != null)
        {
            for (int i = 0; i < multipleEntryDoor.Length; i++)
            {
                if (multipleEntryDoor[i].GetComponent<MultipleEntryDoor>().ActualEntriesSet > 0)
                {
                    multipleEntryDoor[i].GetComponent<MultipleEntryDoor>().SetNewEntry(-1);
                    haveSetAnEntry = false;
                }
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
            emissiveMat.EnableKeyword("_EMISSION");
        }
        if (nbObjectOnThis == 1)
        {
            Instantiate(activationVFX, transform.position, Quaternion.identity);
            CameraShake.Shake(0.05f, 0.2f);
        }
        if (nbObjectOnThis <= 0)
        {
            emissiveMat.DisableKeyword("_EMISSION");

            deactivateEvent.Invoke();
            nbObjectOnThis = 0;
        }
    }
}
