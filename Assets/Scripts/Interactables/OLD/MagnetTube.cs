using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetTube : MonoBehaviour
{
    private GameObject light;
    private GameObject player;
    public GameObject vfxDestroy;
    public GameObject parent;
    private void Start()
    {
        player = GameObject.Find("Player");
        light = GameObject.Find("PlayerLight_v4-1");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == light)
        {
            player.GetComponent<BinaryLight>().DropLight(0f,0f);
            light.GetComponent<Rigidbody>().drag = 10;

            light.transform.position = transform.position;

        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            Instantiate(vfxDestroy, transform.position, Quaternion.identity);
            player.GetComponent<BinaryLight>().GetLight();
            Destroy(parent);


        }
    }

}
