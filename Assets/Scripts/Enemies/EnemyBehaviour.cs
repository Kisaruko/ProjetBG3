using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public bool playerIsInRange = false;
    public float moveSpeed;
    private GameObject player;
    private float minDistanceToAttack =3f;
    public float attackRange;
    public int strength;
    public float recoilInflincted;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(playerIsInRange == true) //si le player a été vu
        {
            transform.LookAt(player.transform); //l'ennemi regarde le joueur
            rb.velocity = (transform.forward.normalized) * moveSpeed; //Il avance toujours vers l'avant
        }
        Attack();
    }

    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // Calcule la distance entre lui meme et le joueur
        if (minDistanceToAttack > distanceToPlayer && playerIsInRange == true) // si le joueur a été vu et est a portée
        { 
            playerIsInRange = false; // L'ennemi s'arrête
            StartCoroutine("Attacking"); //Lance la coroutine d'attaque
        }
    }
    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(1f); // on attends quelques secondes
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, attackRange)) // Draw une sphere devant l'ennemi de radius attackrange
        {
            if (hitcol.gameObject.tag == "Player") //Pour chaque joueur dans la zone
            {
                player.GetComponent<PlayerMovement>().Recoil(transform, recoilInflincted); //Appelle la fonction recoil du joueur et inflige un recul de valeur recoilInflected
                player.GetComponent<PlayerBehaviour>().TakeHit(strength); // Appelle la fonction qui fait perdre des pdv au joueur , le joueur perd 'strength' pdv

            }
        }    
        playerIsInRange = true; // l'ennemi reprend son déplacement
        StopCoroutine("Attacking");
    }
}
