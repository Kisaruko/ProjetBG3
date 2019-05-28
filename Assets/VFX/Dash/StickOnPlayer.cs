using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickOnPlayer : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(this.gameObject, 3f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
    }
}
