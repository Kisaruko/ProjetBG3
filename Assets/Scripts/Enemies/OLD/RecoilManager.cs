using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilManager : MonoBehaviour
{
    private GameObject player;
    public float recoilVelocity;
    private Rigidbody rb;
    public float recoilTime;
    private bool hasBeenHit;
    private MeshRenderer mesh;
    public bool playerIsInRange = false;
    public float divideVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = GetComponent<MeshRenderer>();
    }
    public void TakeHit()
    {
        this.gameObject.GetComponent<LifeManager>().LostLifePoint(1);
        StartCoroutine(Blink(1.0f));
        hasBeenHit = true;
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 recoilDirection = transform.position - player.transform.position;
        rb.velocity = recoilDirection * recoilVelocity;
        StartCoroutine("RecoilTime");
    }
    private void FixedUpdate()
    {
        if (hasBeenHit == false && playerIsInRange == true)
        {
            Vector3 velocity = player.transform.position - transform.position;
            rb.MovePosition(rb.position + velocity.normalized / divideVelocity * Time.deltaTime);
        }
    }
    IEnumerator RecoilTime()
    {
        yield return new WaitForSeconds(recoilTime);
        hasBeenHit = false;
        StopCoroutine("RecoilTime");
    }

    IEnumerator Blink(float waitTime)
    {
        float endTime = Time.time + waitTime;
        while (Time.time < endTime)
        {
            mesh.enabled = false;
            yield return new WaitForSeconds(0.1f);
            mesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine("Blink");
    }
   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerRecoil>().canBeKnockBack == true)
        {
                collision.gameObject.GetComponent<PlayerLife>().PlayerLostLife(-1);
        }
    }*/

}
