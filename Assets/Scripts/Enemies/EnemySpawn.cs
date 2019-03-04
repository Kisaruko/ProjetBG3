using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour

{
    public GameObject enemyPrefab;

    public int nbEnemy;
    private float range =1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector2 spawnPos = this.transform.position;
            for (int i = 0; i < nbEnemy; i++)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnPos = spawnPos + new Vector2(Random.Range(-range, range), (Random.Range(-range, range)));
            }
            Destroy(this.gameObject);
        }
    }
}