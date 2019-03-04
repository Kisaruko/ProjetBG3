using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public int maxLifePoints;
    public int currentLifePoints;
    public GameObject deathFx;

    private void Start()
    {
        currentLifePoints = maxLifePoints;
    }
    public void LostLifePoint(int damageDeal)
    {
        currentLifePoints = currentLifePoints - damageDeal;
        GetComponent<RecoilEnemy>().TakeHit();
        if (currentLifePoints <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        Instantiate(deathFx, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
