using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public bool playerIsInRange = false;
    public float moveSpeed;
    private GameObject player;
    public float minDistanceToAttack =3f;
    private Animator anim;
    public bool isMoving;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if(playerIsInRange) //si le player a été vu
        {
            Chasing();
        }
        DetectIfCanAttack();
    }

    void Chasing()
    {

        isMoving = true;
        anim.SetBool("Chasing", true);
        transform.LookAt(new Vector3(player.transform.position.x, 0f, player.transform.position.z)); //l'ennemi regarde le joueur
        rb.velocity = (transform.forward.normalized) * moveSpeed; //Il avance toujours vers l'avant
    }
    void DetectIfCanAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // Calcule la distance entre lui meme et le joueur
        if (minDistanceToAttack > distanceToPlayer && playerIsInRange == true) // si le joueur a été vu et est a portée
        {
            anim.SetBool("Chasing", false);
            playerIsInRange = false; // L'ennemi s'arrête
            StartCoroutine("Attacking"); //Lance la coroutine d'attaque
        }
    }
    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(1f); // on attends quelques secondes
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(10f); // on attends quelques secondes
        playerIsInRange = true; // l'ennemi reprend son déplacement
        StopCoroutine("Attacking");
    }
}
