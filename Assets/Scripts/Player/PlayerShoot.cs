using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    #region Variables
    [Header("Switching Variables", order = 0)]
    [Space(10, order = 1)]
    public bool isProjectile = true;
    public bool isBeam = false;
    public bool isSwitch;
    public bool canUseBeam;
    public float switchingCooldown;

    [Header("Bullet Variables", order = 0)]
    [Space(10, order = 1)]
    public float lifeUsageOnShoot;
    public GameObject bullet;
    public float bulletSpeed;
    public float loadingTime;
    public float loadedTime;
    private bool isLoading = false;
    private bool alreadyInstantiate = false;

    private Rigidbody rb;
    public float recoilStrength;
    Vector3 lastInput;

    [Header("Laser Variables", order = 0)]
    [Space(10, order = 1)]
    public GameObject laserStart;
    public GameObject laserMiddle;
    public GameObject laserEnd;
    private GameObject start;
    private GameObject middle;
    private GameObject end;
    [Space(5, order = 2)]
    public float lifeUsageToLoadBeam;
    public float lifeUsageEachInterval;
    [Space(5, order = 3)]
    public float laserSize = 20f;
    public int laserDamage;
    [Space(5, order = 4)]
    public float loadedBeamTime;
    private float loadingBeamTime;
    private bool isBeamLoaded;
    [Space(5, order = 5)]
    public float damageInterval;
    private float currentDamageInterval;
    


    [Header("VFX References", order = 0)]
    [Space(10, order = 1)]
    public GameObject loadingFx;
    public GameObject loadedFx;
    private LineRenderer viseur;
    #endregion

    #region Main Methods
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // get le rigidbody
        viseur = GetComponent<LineRenderer>(); // get le line renderer
    }
    private void Update()
    {
        if (Input.GetAxis("Triggers") > 0.5f || Input.GetAxis("Triggers") < -0.5f) //Check if the triggers are pulled
        {
            if (isSwitch == false) //When isSwitch is false it makes it true and launch the coroutine
            {
                isSwitch = true;
                StartCoroutine("SwitchShootMethod");
            }

        }
        //Choosing the shoot method (depends of the coroutine result)
        if (isProjectile)
        {
            ShootProjectile();
        }
        if (isBeam)
        {
            ShootBeam();
        }
    }
    #endregion

    #region Custom Methods
    IEnumerator SwitchShootMethod()
    {
        //Invert the boolean in order to switch the shoot mode
        isBeam = !isBeam;
        isProjectile = !isProjectile;

        //Clear the instantiated objects
        if (isProjectile)
        {
            DestroyLaserGameobjects();
        }

        //Launch a cooldown and makes isSwitch false back to use the switch again
        yield return new WaitForSeconds(switchingCooldown);
        isSwitch = false;
        StopCoroutine("SwitchShootMethod");
    }

    void ShootProjectile()
    {

        float xInput = Input.GetAxis("Horizontal2");
        float yInput = Input.GetAxis("Vertical2");


        if (xInput >= 0.5f || xInput <= -0.5f || yInput >= 0.5f || yInput < -0.5f) // si le joueur touche le joystick droit
        {
            viseur.positionCount = 2; // la ligne fait possede 2 vertex
            viseur.SetPosition(0, transform.position); // le premier point est sur le player
            viseur.SetPosition(1, transform.forward * 40 + transform.position); // le deuxieme point est devant le player + une range de 20
            loadingTime += Time.deltaTime; // augmente le temps de load en fonction du temps
            lastInput = new Vector3(xInput, 0f, yInput); // garde le dernier input en mémoire pour garder la rotation 
            isLoading = true; // le joueur est en train de load un tir
            SfxCheck(); // dois-je instantier des fx?
        }
        else
        {
            viseur.positionCount = 0; // la ligne a 0 vertex, elle n'apparait donc pas
            if (loadingTime >= loadedTime) // si le joueur ne touche pas le joystick est-ce que le joueur a suffisament chargé pour tirer?
            {
                if (GetComponent<PlayerBehaviour>().canShoot == true)// check si le joueur a assez de lumière pour tirer
                {
                    GetComponent<PlayerBehaviour>().UseLifeOnShoot(lifeUsageOnShoot); // utilise de la lumière
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

    void ShootBeam()
    {
        //Did the player unlocked the beam ?
        if (canUseBeam && GetComponent<PlayerBehaviour>().canBeam == true)
        {
            float xInput = Input.GetAxis("Horizontal2");
            float yInput = Input.GetAxis("Vertical2");


            //Build the input detection
            if (xInput >= 0.5f || xInput <= -0.5f || yInput >= 0.5f || yInput < -0.5f)
            {
                //Build aiming before launching the laser
                viseur.positionCount = 2;
                viseur.SetPosition(0, transform.position);
                viseur.SetPosition(1, transform.forward * 40 + transform.position);
                SfxCheck();

                //Charging the laser
                loadingBeamTime += Time.deltaTime;
                isLoading = true;
                if (loadingBeamTime >= loadedBeamTime)
                {
                    viseur.positionCount = 0;
                    isLoading = false;
                    //Execute the life usage only 1 time
                    if(isBeamLoaded == false)
                    {
                        GetComponent<PlayerBehaviour>().UseLifeToLoadBeam(lifeUsageToLoadBeam);
                        isBeamLoaded = true;
                    }

                    SfxCheck();

                    //Call method to use life after an interval
                    currentDamageInterval += Time.deltaTime;
                    if (currentDamageInterval >= damageInterval)
                    {
                        //Call life usage methods
                        GetComponent<PlayerBehaviour>().UseLifeEachInterval(lifeUsageEachInterval);
                        currentDamageInterval = 0f;

                    }


                    //Build the start of the laser beam
                    if (start == null)
                    {
                        start = Instantiate(laserStart) as GameObject;
                        start.transform.parent = this.transform;
                        start.transform.localPosition = Vector3.zero;
                    }
                    //Build the middle of the laser
                    if (middle == null)
                    {
                        middle = Instantiate(laserMiddle) as GameObject;
                        middle.transform.parent = this.transform;
                        middle.transform.localPosition = Vector3.zero;
                    }

                    //Build a raycast with the direction of the laser and its size from the player
                    Vector3 laserDirection = new Vector3(xInput, 0.0f, yInput);

                    //Build the array that store every hit touched with raycastall
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(this.transform.position, laserDirection, laserSize);
                    Debug.DrawRay(transform.position, laserDirection * laserSize, Color.black);

                    //Build the collider detection
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider != null)
                        {
                            if (hits[i].collider.CompareTag("Enemy"))
                            {

                                currentDamageInterval += Time.deltaTime;
                                if (currentDamageInterval >= damageInterval)
                                {
                                    CameraShake.Shake(0.1f, 0.5f);
                                    hits[i].collider.GetComponent<EnemyLife>().LostLifePoint(laserDamage);
                                    currentDamageInterval = 0f;
                                }
                            }
                        }
                    }

                    //Build the end of the laser beam
                    if (end == null)
                    {
                        end = Instantiate(laserEnd) as GameObject;
                        end.transform.parent = this.transform;
                        end.transform.localPosition = Vector3.zero;
                    }

                    //Placing the instatiated objects
                    float startWidth = start.GetComponent<MeshRenderer>().bounds.size.x; //Storing the width of the start object
                    start.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    float endWidth = 0f;
                    if (end != null)
                        endWidth = end.GetComponent<MeshRenderer>().bounds.size.x; //Storing the width of the end object

                    //Build the middle object transform (scale and position) in order to have an object as a laserbeam
                    middle.transform.localScale = new Vector3(middle.transform.localScale.x, middle.transform.localScale.y, laserSize - startWidth);
                    middle.transform.localPosition = new Vector3(0f, 0f, laserSize / 2f);
                    middle.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

                    //Build the end object position and rotation
                    if (end != null)
                    {
                        end.transform.localPosition = new Vector3(0f, 0f, laserSize);
                        end.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    }

                }
            }
            //When the player is not using the right stick
            else
            {
                //Reset the variables and destroy the laser gameobjects
                isBeamLoaded = false;
                loadingBeamTime = 0f;
                viseur.positionCount = 0;
                DestroyLaserGameobjects();
            }
        }
        if (!GetComponent<PlayerBehaviour>().canBeam)
        {
            DestroyLaserGameobjects();
        }
    }

    private void DestroyLaserGameobjects()
    {
        Destroy(start);
        Destroy(middle);
        Destroy(end);
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

    #endregion
}
