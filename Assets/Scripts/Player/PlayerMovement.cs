using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public bool controlsAreEnabled;
    public float sensitivity;
    [Header("Movement Variables", order = 0)]
    public bool isMoving = false;
    public float moveSpeed;
    private Rigidbody rb;
    private float BaseSpeed;
    public float moveSpeedWhileAiming;
    private CustomGravity customgravity;
    private Animator anim;
    public float rotationSpeed;

    [Header("Dash Variables")]
    public float lifeUsageOnDash;
    public bool isDashing;
    public bool isReadyToDash;
    public float dashingTime;
    public float coolDown;
    public float dashSpeed;
    Quaternion lastRotation;
    private PlayerBehaviour playerbehaviour;

    [Header("Upgrade Dash Variables")]
    public bool upgradeDashUnlocked;
    public float loadedDash;
    public float loadingDash;
    private int numbersOfDash = 0;
    public int maxNumbersOfDash;


    [Header("Recoil Variables")]
    public bool isRecoiling = false;
    public float recoilDuration;

    [Header("Particles Variables")]
    public GameObject trailDashParticles;
    #endregion

    #region Main Methods
    private void Start()
    {
        playerbehaviour = GetComponent<PlayerBehaviour>();
        customgravity = GetComponent<CustomGravity>();
        rb = GetComponent<Rigidbody>();
        BaseSpeed = moveSpeed;
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (controlsAreEnabled)
        {
            Movement();
            DashDetection();
        }
    }
    #endregion

    #region Custom Methods
    void Movement()
    {
        float xInput = Input.GetAxis("Horizontal")* sensitivity; //Joystick gauche horizontal
        float yInput = Input.GetAxis("Vertical")* sensitivity; //Joystick gauche vertical
        float xInput2 = Input.GetAxis("Horizontal2"); //Joystick droit horizontal
        float yInput2 = Input.GetAxis("Vertical2"); //Joystick droit vertical

        Vector3 lookDirection = new Vector3(xInput, 0f, yInput); // direction du joystick gauche
        Vector3 lookDirection2 = new Vector3(xInput2, 0f, yInput2); // direction du joystick droit

        if (isRecoiling == false) // si le joueur ne prend pas un recul
        {
            if (xInput >= 0.1f || xInput <= -0.1f || yInput >= 0.1f || yInput < -0.1f && isDashing == false) // si le joueur bouge mais ne dash pas
            {
                isMoving = true;//il bouge
                anim.SetBool("isMoving", true);
                if (lookDirection2 == Vector3.zero) // si le joueur ne touche pas au joystick droit
                {
                    Quaternion smoothRotation = Quaternion.LookRotation(lookDirection);
                    //transform.rotation = Quaternion.LookRotation(looksDirection, Vector3.up); // le joueur regarde en face de lui
                    transform.rotation = Quaternion.Slerp(lastRotation, smoothRotation, rotationSpeed);
                }
                rb.velocity = new Vector3(xInput, 0f, yInput) * moveSpeed; // le joueur avance dans la direction du joystick gauche
                lastRotation = transform.rotation; //Enregistre le dernier input du joueur pour qu'il regarde dans la dernière direction dans laquelle il allait
            }

            else  // si le joueur ne bouge pas
            {
                anim.SetBool("isMoving", false);
                isMoving = false;
                rb.velocity = Vector3.zero;  // la vitesse du joueur est de 0
                transform.rotation = lastRotation; // le joueur regarde dans la dernière direction enregistrée
            }

            if (xInput2 >= 0.5f || xInput2 <= -0.5f || yInput2 >= 0.5f || yInput2 < -0.5f) // si le joueur touche le joystick droit
            {
                //transform.rotation = Quaternion.LookRotation(lookDirection2, Vector3.up); // il regarde dans la direction du joystick droit: ça override l'autre joystick
                //lastRotation = transform.rotation; //le joueur regarde dans la derniere direction de l'input
                //moveSpeed = moveSpeedWhileAiming;
            }
            else
            {
                moveSpeed = BaseSpeed; // reviens à la vitesse originelle
            }
        }
    }
    public void DashDetection()
    {
        if (Input.GetButtonDown("Dash") && isReadyToDash == true && isRecoiling == false) // si le joueur peut dasher, qu'il ne subit pas de recul et qu'il appuie sur l'input
        {

            if (playerbehaviour.canDash == true) // si le joueur a assez de lumière pour dasher
            {
                anim.SetBool("isDashing", true);
                Instantiate(trailDashParticles, transform.position, Quaternion.identity);
                playerbehaviour.UseLifeOnDash(lifeUsageOnDash); //consomme de la lumière

                isReadyToDash = false; // le joueur ne peut pas redasher
                isDashing = true; // le joueur est en train de dasher
                StartCoroutine("DashTime"); // on lance la coroutine du cooldown du dash
            }
        }
        Dash();

        /*                                        /!\ DASH CRANTE NE PAS SUPPRIMER
        if (upgradeDashUnlocked)
        {
            if (Input.GetButton("Dash"))
            {
                loadingDash += Time.deltaTime;
                if (loadingDash > loadedDash)
                {
                    loadingDash = 0.0f;
                    numbersOfDash++;
                    Debug.Log(numbersOfDash);
                    if (numbersOfDash >= maxNumbersOfDash)
                    {
                        for (int i = 0; i < maxNumbersOfDash; i++)
                        {
                            isReadyToDash = false;
                            isDashing = true;
                            StartCoroutine("DashTime");
                            Dash();
                        }
                        numbersOfDash = 0;
                        loadingDash = 0.0f;
                    }
                }

            }
        }*/

    }

    private void Dash()
    {
        if (isDashing == true) //si le joueur est en train de dasher
        {

            //Build the dash direction and velocity
            Vector3 dashDirection = transform.forward;
            Vector3 dashVelocity = dashDirection * dashSpeed;

            //Build the raycast
            RaycastHit hit;

            //Use the translate method (movement without collider) if an enemy hit in the ray cast
            if (Physics.Raycast(transform.position, dashVelocity, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    transform.Translate(dashVelocity * Time.deltaTime, Space.World);
                }
                //Use rigidbody for other dash movement in order to not pass through walls
                else
                {
                    rb.velocity = dashVelocity;
                }
            }
            customgravity.gravityScale = 0f; // desactive la gravité pendant le dash
        }
        else
        {
            customgravity.gravityScale = 50f; // réactive la gravité
        }
    }

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(dashingTime); // le temps que le dash dure
        anim.SetBool("isDashing", false);
        isDashing = false; // le joueur ne dash plus
        yield return new WaitForSeconds(coolDown); //lance le cooldown
        isReadyToDash = true; // le joueur peut redasher
        StopCoroutine("DashTime");// stop la coroutine
    }

    public void Recoil(Transform enemy, float recoilSpeed)
    {
        if (playerbehaviour.isInvicible == false) // si le joueur n'est pas invincible
        {
            transform.rotation = lastRotation; // la rotation = le dernier input sur joystick enregistré
            isRecoiling = true; //le joueur recul
            Vector3 recoilDirection = (enemy.position - transform.position).normalized; // calcule la direction entre le player et l'ennemi
            rb.velocity = (recoilDirection * recoilSpeed) * -1; // fait reculer le player par rapport à l'ennemi
            StartCoroutine("RecoilTime");

        }
    }
    IEnumerator RecoilTime()
    {
        yield return new WaitForSeconds(recoilDuration); // attends 'recoilduration"
        isRecoiling = false; // le joueur ne recule plus
        StopCoroutine("RecoilTime"); // stop recule

    }
    #endregion
}
