using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilEnemy : MonoBehaviour
{
    private GameObject player;
    public float recoilVelocity;
    private Rigidbody rb;
    public float recoilTime;
    private MeshRenderer mesh;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = GetComponent<MeshRenderer>();
    }
    public void TakeHit()
    {
        GetComponent<EnemyBehaviour>().playerIsInRange = false; //arrêt du mouvement du monstre
        StartCoroutine(Blink(1.0f)); //Le monstre clignote pour signaler qu'il a pris un coup
        player = GameObject.FindGameObjectWithTag("Player"); // get le player
    }
    public IEnumerator RecoilTime()
    {
        Vector3 recoilDirection = transform.position - player.transform.position; //calcul de la direction du recul
        rb.velocity = recoilDirection * recoilVelocity; // calcule et execute le recul 
        yield return new WaitForSeconds(recoilTime);// attendre la durée du recul
        GetComponent<EnemyBehaviour>().playerIsInRange = true; // relance le mouvement normal du monstre
        StopCoroutine("RecoilTime");// arrêt de la coroutine
    }
    IEnumerator Blink(float waitTime)
    {
        float endTime = Time.time + waitTime; // durée du blink
        while (Time.time < endTime)// tant que le temps n'est pas supérieur au temps indiqué pour blink
        {
            mesh.enabled = false; // desactive le mesh
            yield return new WaitForSeconds(0.1f); // attends un peu
            mesh.enabled = true; // réactive le mesh
            yield return new WaitForSeconds(0.1f); // attends un peu
        }
        StopCoroutine("Blink"); // arrêt de la coroutine
    }
}
