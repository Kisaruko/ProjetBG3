using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    private Rigidbody rb;
    public float loadingTime;
    public float loadedTime;
    public GameObject bullet;
    public float recoilStrength;
    public float bulletSpeed;
    Vector3 lastInput;
    private bool isLoading = false;
    private bool alreadyInstantiate = false;
    public GameObject loadingFx;
    public GameObject loadedFx;
    private LineRenderer lineRenderer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // get le rigidbody
        lineRenderer = GetComponent<LineRenderer>(); // get le line renderer
    }
    private void Update()
    {
        ShootBeam();
    }
    void ShootBeam()
    {

        float xInput = Input.GetAxis("Horizontal2");
        float yInput = Input.GetAxis("Vertical2");


        if (xInput >= 0.5f || xInput <= -0.5f || yInput >= 0.5f || yInput <-0.5f) // si le joueur touche le joystick droit
        {
            lineRenderer.SetVertexCount(2); // la ligne fait possede 2 vertex
            lineRenderer.SetPosition(0, transform.position); // le premier point est sur le player
            lineRenderer.SetPosition(1, transform.forward * 40 + transform.position); // le deuxieme point est devant le player + une range de 20
            loadingTime += Time.deltaTime; // augmente le temps de load en fonction du temps
            lastInput = new Vector3(xInput, 0f, yInput); // garde le dernier input en mémoire pour garder la rotation 
            isLoading = true; // le joueur est en train de load un tir
            SfxCheck(); // dois-je instantier des fx?
        }
        else
        {
            lineRenderer.SetVertexCount(0); // la ligne a 0 vertex, elle n'apparait donc pas
            if (loadingTime >= loadedTime) // si le joueur ne touche pas le joystick est-ce que le joueur a suffisament chargé pour tirer?
            {
                if (GetComponent<PlayerBehaviour>().canShoot == true)// check si le joueur a assez de lumière pour tirer
                {
                    GetComponent<PlayerBehaviour>().UseLifeOnShoot(); // utilise de la lumière
                    rb.AddForce((transform.forward * -1) * recoilStrength, ForceMode.Impulse); // le joueur recule a cause du tir
                    GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity); // j'instantie un clone de ma balle pour pouvoir la modifier elle et seulement elle
                    clone.GetComponent<Rigidbody>().velocity = lastInput.normalized * bulletSpeed; // je calcule la velocité de ma balle
                }
            }
            isLoading = false;// le joueur a laché l'input, il ne charge plus
            loadingTime = 0f; // le temps de load revient a 0
            alreadyInstantiate = false; // les fx peuvent être réinstantiés
            SfxCheck(); // je check si je peux balancer mes fx

        }
    }
    void SfxCheck()
    {
        if (isLoading == true && GetComponent<PlayerBehaviour>().canShoot == true) // si je peux tirer
        {
            loadingFx.SetActive(true); // activer le fx de load
        }
        else
        {
            loadingFx.SetActive(false); // desactiver le fx de load

        }
        if (loadingTime >= loadedTime && alreadyInstantiate == false && GetComponent<PlayerBehaviour>().canShoot == true) //si le tir est chargé et que le joueur peut tirer
        {
            Instantiate(loadedFx, transform.position, Quaternion.identity); //j'instantie le fx qui prévient que le tir est chargé
            alreadyInstantiate = true; // le fx a déjà été instantié
        }

    }
}

