using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithPoints : MonoBehaviour
{
    #region Variables
    public GameObject gameObjectToSpawn;
    public float detectionRange;
    public LayerMask spawners;
    public int pointDivision =1;
    public float timeBeforeInvoking;
    #endregion

    #region Main Methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))// si le player declenche le spawn
        {
            Invoke("InvokeEnemies", timeBeforeInvoking);// invoque ennemies
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
        List<Transform> spawnPoints = new List<Transform>(); // new list
        foreach(Collider spawnpoints in Physics.OverlapSphere(transform.position,detectionRange,spawners)) // répertorie tout les spawners dans la range de detection
        {
            spawnPoints.Add(spawnpoints.transform);// add un composant a la liste
        }
        return spawnPoints; // retourne la liste de tout les spawners dans la range

    }
    private void InvokeEnemies()
    {
        List<Transform> touchedSpawners = GetSpawnersInRange(); // recupere la liste générée par l'autre fonction
        Transform[] spawnerArray = touchedSpawners.ToArray(); // Transforme la liste en Tableau
        /*for (int i = 0; i < spawnerArray.Length/pointDivision; i++) // instantier un fx d'annonciation
        {
            Instantiate(//Fx annonciateur de chaos, spawnerArray[i].transform.position, Quaternion.identity);          
        }*/
        for (int i = 0; i < spawnerArray.Length/pointDivision; i++) // Pour chaque spawnpoint dans le tableau, point division est une valeur qui sert à instantier les objets sur un certain pourcentage de points détectés et non tous
        {
            Instantiate(gameObjectToSpawn, spawnerArray[i].transform.position, Quaternion.identity); // instantie l'objet sur un spawnpoint
            Destroy(spawnerArray[i].gameObject); // detruit le spawn point sur lequel l'objet vient d'etre instantié
        }
        Destroy(this.gameObject);// quand la boucle est finie, détruire le spawner
    }
    #endregion
}

