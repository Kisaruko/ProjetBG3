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
        GetComponent<EnemyBehaviour>().playerIsInRange = false;
        StartCoroutine(Blink(1.0f));;
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 recoilDirection = transform.position - player.transform.position;
        rb.velocity = recoilDirection * recoilVelocity;
        StartCoroutine("RecoilTime");
    }
    IEnumerator RecoilTime()
    {
        yield return new WaitForSeconds(recoilTime);
        GetComponent<EnemyBehaviour>().playerIsInRange = true;
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
}
