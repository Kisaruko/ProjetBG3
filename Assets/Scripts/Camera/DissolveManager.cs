using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveManager : MonoBehaviour
{
    private GameObject target;
    public bool isBehindWall;
    public Material mat;
    public float maxRange;
    public float dissolveSpeed;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    void ClippingCheck()
    {
        Vector3 playerPos = target.transform.position; // get la position de la cible
        Debug.DrawRay(playerPos, transform.position - playerPos, Color.black); // dessine le ray dans l'éditeur
        RaycastHit hit; //crée le raycast
        if (Physics.Raycast(playerPos, transform.position - playerPos, out hit, maxRange)) // le ray ne touche pas le joueur
        {
            isBehindWall = true; // le joueur est derriere un mur
            mat.SetFloat("_Amount", Mathf.Lerp(mat.GetFloat("_Amount"), 1f, dissolveSpeed)); // je dissolve l'objet    
        }
        else
        {
            isBehindWall = false;// le joueur n'est pas derrière un mur
            mat.SetFloat("_Amount", Mathf.Lerp(mat.GetFloat("_Amount"), 0f, dissolveSpeed)); // je reconstruit l'objet
        }

    }
    private void FixedUpdate()
    {
        ClippingCheck(); 
    }
}
