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
    private Animator anim;
    public float rotationSpeed;
    private CapsuleCollider collider;

    [Header("Dash Variables")]
    public float lifeUsageOnDash;
    public bool isDashing;
    public bool isReadyToDash;
    public float dashingTime;
    public float coolDown;
    public float dashSpeed;
    Quaternion lastRotation;
    private BinaryLight binaryLight;
    private LightManager lightManager;

    [Header("Upgrade Dash Variables")]
    public bool upgradeDashUnlocked;
    public float loadedDash;
    public float loadingDash;
    public int maxNumbersOfDash;


    [Header("Recoil Variables")]
    public bool isRecoiling = false;
    public float recoilDuration;

    [Header("Particles Variables")]
    public GameObject trailDashParticles;
    public ParticleSystem shinyBody;
    #endregion

    #region Main Methods
    private void Start()
    {
        binaryLight = GetComponent<BinaryLight>();
        lightManager = GetComponentInChildren<LightManager>();
        rb = GetComponent<Rigidbody>();
        BaseSpeed = moveSpeed;
        anim = GetComponentInChildren<Animator>();
        collider = GetComponentInChildren<CapsuleCollider>();
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
        float xInput = Input.GetAxis("Horizontal") * sensitivity; //Joystick gauche horizontal
        float yInput = Input.GetAxis("Vertical") * sensitivity; //Joystick gauche vertical
        float xInput2 = Input.GetAxis("Horizontal2"); //Joystick droit horizontal
        float yInput2 = Input.GetAxis("Vertical2"); //Joystick droit vertical

        Vector3 lookDirection = new Vector3(xInput, 0f, yInput); // direction du joystick gauche
        Vector3 lookDirection2 = new Vector3(xInput2, 0f, yInput2); // direction du joystick droit
        float animSpeed = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        anim.SetFloat("Speed", animSpeed);

        if (isRecoiling == false && isDashing == false) // si le joueur ne prend pas un recul
        {
            if (xInput >= 0.1f || xInput <= -0.1f || yInput >= 0.1f || yInput < -0.1f) // si le joueur bouge mais ne dash pas
            {
                isMoving = true;//il bouge
                anim.SetBool("isMoving", true);
                if (lookDirection2 == Vector3.zero) // si le joueur ne touche pas au joystick droit
                {
                    Quaternion smoothRotation = Quaternion.LookRotation(lookDirection);
                    //transform.rotation = Quaternion.LookRotation(looksDirection, Vector3.up); // le joueur regarde en face de lui
                    transform.rotation = Quaternion.Slerp(lastRotation, smoothRotation, rotationSpeed);
                }

                Vector3 Velocity = new Vector3(xInput, -0.35f, yInput);
                Velocity.Normalize();
                rb.velocity = Velocity * moveSpeed; // le joueur avance dans la direction du joystick gauche
                lastRotation = transform.rotation; //Enregistre le dernier input du joueur pour qu'il regarde dans la dernière direction dans laquelle il allait
            }

            else  // si le joueur ne bouge pas
            {
                anim.SetBool("isMoving", false);
                isMoving = false;

                rb.velocity = Vector3.down * moveSpeed;  // la vitesse du joueur est de 0

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
        if (Input.GetButtonDown("Dash") || Input.GetMouseButtonDown(1) && isReadyToDash == true && isRecoiling == false) // si le joueur peut dasher, qu'il ne subit pas de recul et qu'il appuie sur l'input
        {
            if (lightManager.canDash == true) // si le joueur a assez de lumière pour dasher // Remplacer par le candash de binarylight
            {
                lightManager.LightDecreasing();
                lightManager.canDash = false;
                shinyBody.Play();
                anim.SetBool("isDashing", true);
                Instantiate(trailDashParticles, transform.position, Quaternion.identity);

                isReadyToDash = false; // le joueur ne peut pas redasher
                isDashing = true; // le joueur est en train de dasher
                Dash();
                StartCoroutine("DashTime"); // on lance la coroutine du cooldown du dash
            }
            else
            {
                //dash Echec
            }

        }
    }
    private void Dash()
    {
        if (isDashing) //si le joueur est en train de dasher
        {
            //Use rigidbody for other dash movement in order to not pass through walls
            rb.velocity = transform.forward * dashSpeed;
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
        if (binaryLight.isInvicible == false) // si le joueur n'est pas invincible // remplacer par inviciblité sur 
        {
            anim.SetBool("TakeHit", true);
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
        anim.SetBool("TakeHit", false);

        StopCoroutine("RecoilTime"); // stop recule

    }
    #endregion
}

