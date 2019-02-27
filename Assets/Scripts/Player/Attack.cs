using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Variables
    public GameObject fxAttack;
    public int strength;
    public float range;
    public GameObject stealLightFx;
    #endregion


    #region Main Methods
    private void Update()
    {
        DoAttack();
    }
    #endregion

    #region Custom Methods
    void DoAttack()
    {
        if (Input.GetButtonDown("Fire1")) // Si le joueur appuie sur l'input d'attack
        {
            Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
            foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, range)) // Crée une sphère devant le joueur de radius range
            {
                if (hitcol.gameObject.tag == "Enemy") // pour chaque ennemi dans la sphere
                {
                    GetComponent<PlayerBehaviour>().RegenLifeOnCac(); // appelle la fonction regen de lumière
                    hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
                    Instantiate(stealLightFx, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transform.position+ transform.forward, range);
    }
    #endregion
}
