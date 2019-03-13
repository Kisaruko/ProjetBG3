using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject gameObjectToSpawn;
    public int nbObjectToSpawn;
    public float range = 1f;
    public bool spawnInTheAir = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // si le joueur touche cet objet
        {
            Vector3 spawnPos = this.transform.position;  //endroit de base du spawner
            for (int i = 0; i < nbObjectToSpawn; i++) // Répéter nbEnemyValue l'action
            {
                if (spawnInTheAir == false) // Spawner au niveau du y du spawner
                {
                    spawnPos = new Vector3(Random.Range(transform.position.x + range, transform.position.x - range), transform.position.y, Random.Range(transform.position.z + range, transform.position.z - range));
                }
                else // autoriser le a spawner au dessus du spawner
                {
                    spawnPos = new Vector3(Random.Range(transform.position.x + range, transform.position.x - range), Random.Range(transform.position.y + range, transform.position.y), Random.Range(transform.position.z + range, transform.position.z - range));
                }
                Instantiate(gameObjectToSpawn, spawnPos, Quaternion.identity); // instantier un enemy sur le spawnpos              
            }
            Destroy(this.gameObject); // detruit l'objet, on en a plus besoin
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
