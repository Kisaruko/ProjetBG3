using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving = false;
    public float moveSpeed;
    private float BaseSpeed;
    public bool isDashing;
    public bool isReadyToDash;
    public float dashingTime;
    public float coolDown;
    public float dashSpeed;
    Quaternion lastRotation;
    public bool isRecoiling = false;
    public float recoilDuration;
    private ParticleSystem ps;
    public float moveSpeedWhileAiming;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
        BaseSpeed = moveSpeed;
    }
    private void FixedUpdate()
    {
        Movement();
        Dashing();
    }
    void Movement()
    {
        float xInput = Input.GetAxis("Horizontal"); //Joystick gauche horizontal
        float yInput = Input.GetAxis("Vertical"); //Joystick gauche vertical
        float xInput2 = Input.GetAxis("Horizontal2"); //Joystick droit horizontal
        float yInput2 = Input.GetAxis("Vertical2"); //Joystick droit vertical

        Vector3 lookDirection = new Vector3(xInput,0f,yInput); // direction du joystick gauche
        Vector3 lookDirection2 = new Vector3(xInput2,0f,yInput2); // direction du joystick droit

        if (isRecoiling == false) // si le joueur ne prend pas un recul
        {
            if (xInput != 0 || yInput != 0 && isDashing == false) // si le joueur bouge mais ne dash pas
            {
                isMoving = true;//il bouge
                if (lookDirection2 == Vector3.zero) // si le joueur ne touche pas au joystick droit
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up); // le joueur regarde en face de lui
                }
                rb.velocity = new Vector3(xInput, 0f, yInput) * moveSpeed; // le joueur avant dans la direction du joystick gauche
                lastRotation = transform.rotation; //Enregistre le dernier input du joueur pour qu'il regarde dans la dernière direction dans laquelle il allait
            }

            else  // si le joueur ne bouge pas
            {
                isMoving = false;
                rb.velocity = Vector3.zero;  // la vitesse du joueur est de 0
                transform.rotation = lastRotation; // le joueur regarde dans la dernière direction enregistrée
            }

            if (lookDirection2 != Vector3.zero) // si le joueur touche le joystick droit
            {
                transform.rotation = Quaternion.LookRotation(lookDirection2, Vector3.up); // il regarde dans la dirdction du joystick droit: ça override l'autre joystick
                lastRotation = transform.rotation; //le joueur regarde dans la derniere direction de l'input
                moveSpeed = moveSpeedWhileAiming;
            }
            else
            {
                moveSpeed = BaseSpeed; // reviens à la vitesse originelle
            }
        }
    }
    void Dashing()
    {
        if(Input.GetButtonDown("Dash") && isReadyToDash == true && isRecoiling == false) // si le joueur peut dasher, qu'il ne subit pas de recul et qu'il appuie sur l'input
        {
            if (GetComponent<PlayerBehaviour>().canDash == true) // si le joueur a assez de lumière pour dasher
            {
                ps.enableEmission = true; // active l'emission du fx de dash

                GetComponentInChildren<DissolveEffect>().dissolve = true; // dissolve le joueur
                GetComponentInChildren<DissolveEffect>().ressolve = false; // desactive le ressolve du joueur
                GetComponent<PlayerBehaviour>().UseLifeOnDash(); //consomme de la lumière
                isReadyToDash = false; // le joueur ne peut pas redasher
                isDashing = true; // le joueur est en train de dasher
                StartCoroutine("DashTime"); // on lance la coroutine du cooldown du dash
            }

        }
        if(isDashing == true) //si le joueur est en train de dasher
        {
            rb.velocity = transform.forward * dashSpeed; // il dash et sa vitesse augmente 
            GetComponent<CustomGravity>().gravityScale = 0f; // desactive la gravité pendant le dash

        }
        else
        {
            GetComponent<CustomGravity>().gravityScale = 50f; // réactive la gravité

        }
    }
    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(dashingTime); // le temps que le dash dure
        isDashing = false; // le joueur ne dash plus

        GetComponentInChildren<DissolveEffect>().dissolve = false; // desactive le dissolve du player
        GetComponentInChildren<DissolveEffect>().ressolve = true; // fait réapparaitre le player
        yield return new WaitForSeconds(coolDown); //lance le cooldown
        isReadyToDash = true; // le joueur peut redasher
        ps.enableEmission = false; // arrete l'emission de particules de dash

        StopCoroutine("DashTime");// stop la coroutine
    }

    public void Recoil(Transform enemy, float recoilSpeed)
    {
        if (GetComponent<PlayerBehaviour>().isInvicible == false) // si le joueur n'est pas invincible
        {
            transform.rotation = lastRotation; // la rotation = le dernier input sur joystick enregistré
            isRecoiling = true; //le joueur recul
            Vector3 recoilDirection = (enemy.position - transform.position).normalized; // calcule la direction entre le player et l'ennemi
            rb.velocity = (recoilDirection * recoilSpeed) * -1; // fait reculer le player par rapport à l'ennemi
            StartCoroutine("RecoilTime")

        }
    }
    IEnumerator RecoilTime()
    {
       yield return new WaitForSeconds(recoilDuration); // attends 'recoilduration"
        isRecoiling = false; // le joueur ne recule plus
        StopCoroutine("RecoilTime"); // stop recule
       
    }

}
