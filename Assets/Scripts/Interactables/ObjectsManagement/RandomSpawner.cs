using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    #region Variables
    public GameObject gameObjectToSpawn;
    public int nbObjectToSpawn;
    public float range = 1f;
    public bool spawnInTheAir = false;
    private bool hasBeenActivated = false;
    public bool activateAtStart;
    #endregion

    #region Methods
    private void Start()
    {
        if (activateAtStart)
        {
            Spawn();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasBeenActivated == false) // si le joueur touche cet objet
        {
            Spawn();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    #endregion
    private void Spawn()
    {
        hasBeenActivated = true;
        Vector3 spawnPos = this.transform.position;  //endroit de base du spawner
        for (int i = 0; i < nbObjectToSpawn; i++) // Répéter nbEnemyValue l'action
        {
            if (spawnInTheAir == false) // Spawner au niveau du y du spawner
            {
                spawnPos = new Vector3(Random.Range(transform.position.x + range, transform.position.x - range), transform.position.y, Random.Range(transform.position.z + range, transform.position.z - range));// randomise la position du spawner aléatoirement dans la range sans Y
            }
            else // autoriser le a spawner au dessus du spawner
            {
                spawnPos = new Vector3(Random.Range(transform.position.x + range, transform.position.x - range), Random.Range(transform.position.y + range, transform.position.y), Random.Range(transform.position.z + range, transform.position.z - range));// randomise la position du spawner aléatoirement dans la range avec +Y uniquement
            }
            Instantiate(gameObjectToSpawn, spawnPos, Quaternion.identity); // instantier un enemy sur le spawnpos  

        }
        Destroy(this.gameObject); // detruit l'objet, on en a plus besoin
    }
}
