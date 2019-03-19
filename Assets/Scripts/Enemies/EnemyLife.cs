using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public int maxLifePoints;
    public int currentLifePoints;
    public GameObject deathFx;
    public GameObject hasBeenHitFx;
    private RecoilEnemy recoilenemy;
    private void Start()
    {
        currentLifePoints = maxLifePoints; // les pdv de départ de l'enemy sont au maximum
        recoilenemy = GetComponent<RecoilEnemy>();
    }
    public void LostLifePoint(int damageDeal) //DamageDeal est une valeur qui doit être rentrée lors de l'appel de cette fonction et elle indique le nombre de pdv infligés à l'ennemi
    {
        Vector3 pointToInstantiate = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(hasBeenHitFx,pointToInstantiate , Quaternion.identity);// instantier le fx de mort
        currentLifePoints = currentLifePoints - damageDeal;  // les pdv sont égaux aux pdv actuels - les dommage causées lors de l'appel de la fonction
        recoilenemy.TakeHit(); // Appel la fonction recul de l'ennemi
        if (currentLifePoints <= 0) // si les pv sont inférieurs ou égaux a 0
        {
            Death(); // l'ennemi meurt
        }
    }
    void Death()
    {
        Instantiate(deathFx, transform.position, Quaternion.identity);// instantier le fx de mort
        Destroy(this.gameObject);//detruire l'objet
    }
}
