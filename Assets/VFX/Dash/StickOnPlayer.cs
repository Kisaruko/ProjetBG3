using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickOnPlayer : MonoBehaviour
{
    private GameObject player;
    private ParticleSystem ps;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ps = GetComponent<ParticleSystem>();
        var sh = ps.shape;
        sh.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
        sh.skinnedMeshRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        Destroy(this.gameObject, 3f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }
}
