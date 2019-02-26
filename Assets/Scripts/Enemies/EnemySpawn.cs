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
        if (other.gameObject.tag == "Player") // si le joueur touche cet objet
        {
            Vector2 spawnPos = this.transform.position;  //endroit de base du spawner
            for (int i = 0; i < nbEnemy; i++) // Répéter nbEnemyValue l'action
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity); // instantier un enemy sur le spawnpos
                spawnPos = spawnPos + new Vector2(Random.Range(-range, range), (Random.Range(-range, range))); //Changer le spawnpos aléatoirement
            }
            Destroy(this.gameObject); // detruit l'objet, on en a plus besoin
        }
    }
}