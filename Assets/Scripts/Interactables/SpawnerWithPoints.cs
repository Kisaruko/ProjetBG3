using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithPoints : MonoBehaviour
{
    public GameObject gameObjectToSpawn;
    public float detectionRange;
    public LayerMask spawners;

    #region Main Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InvokeEnemies();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    #endregion
    #region Custom Methods

    public List<Transform> GetSpawnersInRange()
    {
        List<Transform> spawnPoints = new List<Transform>();
        foreach(Collider spawnpoints in Physics.OverlapSphere(transform.position,detectionRange,spawners))
        {
            spawnPoints.Add(spawnpoints.transform);// add un composant a la liste
        }
        return spawnPoints;

    }
    private void InvokeEnemies()
    {
        List<Transform> touchedSpawners = GetSpawnersInRange();
        Transform[] spawnerArray = touchedSpawners.ToArray();
        /*for (int i = 0; i < spawnerArray.Length; i++) // instantier un fx d'annonciation
        {
            Instantiate(//Fx annonciateur de chaos, spawnerArray[i].transform.position, Quaternion.identity);          
        }*/
        for (int i = 0; i < spawnerArray.Length; i++)
        {
            Instantiate(gameObjectToSpawn, spawnerArray[i].transform.position, Quaternion.identity);
            Destroy(spawnerArray[i].gameObject);
        }
        Destroy(this.gameObject);
    }
    #endregion
}

