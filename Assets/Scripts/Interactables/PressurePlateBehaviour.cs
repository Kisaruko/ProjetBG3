using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateBehaviour : MonoBehaviour
{
    [Header("Activation Variables")]
    public List<string> activationTag;
    public bool isActivated;
    public GameObject[] associatedGameobject;

    [Header("Pressure Plate Variables")]
    public float pressureFactorY;
    private float startingPosY;

    [Header("Emission Variables")]
    public Color baseColor;
    private Material material;
    private Color finalColor;
    private float emission;
    
    void Start()
    {
        material = GetComponent<Renderer>().material; //Get the material in order to change its Emissive color
        startingPosY = transform.position.y; //Get the position Y in order to change it for when it has pressure on it
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
                for (int i = 0; i < associatedGameobject.Length; i++)
                {
                    associatedGameobject[i].GetComponent<ActivableDoorBehaviour>().Activate();
                }
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
                for (int i = 0; i < associatedGameobject.Length; i++)
                {
                    associatedGameobject[i].GetComponent<ActivableDoorBehaviour>().Deactivate();
                }
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
}
