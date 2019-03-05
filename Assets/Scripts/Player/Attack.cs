using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Variables
    [Header("Values", order = 0)]
    [Space(10, order = 1)]
    public int strength;
    public float range;
    public float speedWhileAttacking;
    [Header("Vfx", order = 0)]
    [Space(10, order = 1)]
    public GameObject stealLightFx;
    public GameObject fxAttack;
    public PlayerMovement playermovement;
    private Animator anim;
    public int noOfClicks = 0;
    //Time when last button was clicked
    float lastClickedTime = 0;
    //Delay between clicks for which clicks will be considered as combo
    float maxComboDelay = 1;

    #endregion

    #region Main Methods
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playermovement = GetComponentInParent<PlayerMovement>();
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            OnButtonClick();
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack3", false);
        }
        if(noOfClicks != 0)
        {
            playermovement.moveSpeed = speedWhileAttacking;
        }
    }
    #endregion
    #region Custom Methods
    
    #region InputDetection
    void OnButtonClick()
    {
        //Record time of last button click
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            anim.SetBool("Attack1", true);
        }
        //limit/clamp no of clicks between 0 and 3 because you have combo for 3 clicks
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        Debug.Log(noOfClicks);
    }

    public void ComboCheck1()
    {
        anim.SetBool("Attack1", false);
        if (noOfClicks>= 2 )
        {
            anim.SetBool("Attack2", true);

        }
        else
        {
            noOfClicks = 0;
        }
    }
    public void ComboCheck2()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        if (noOfClicks >= 3)
        {
            anim.SetBool("Attack3", true);
            noOfClicks = 0;

        }
    }
    #endregion

    #region AttackBehaviour 
    public void Attack1()
    {
        Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, range)) // Crée une sphère devant le joueur de radius range
        {
            if (hitcol.gameObject.tag == "Enemy") // pour chaque ennemi dans la sphere
            {
                hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
                Instantiate(stealLightFx, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
            }
        }
    }

    public void Attack2()
    {
        Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, range)) // Crée une sphère devant le joueur de radius range
        {
            if (hitcol.gameObject.tag == "Enemy") // pour chaque ennemi dans la sphere
            {
                hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
                Instantiate(stealLightFx, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
            }
        }
    }

    public void Attack3()
    {
        Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, range)) // Crée une sphère devant le joueur de radius range
        {
            if (hitcol.gameObject.tag == "Enemy") // pour chaque ennemi dans la sphere
            {
                hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength+1); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
                Instantiate(stealLightFx, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
                Instantiate(stealLightFx, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
            }
        }
    }

    /*  void DoAttack()
       {
           if (Input.GetButtonDown("Attack")) // Si le joueur appuie sur l'input d'attack
           {
               Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
               foreach (Collider hitcol in Physics.OverlapSphere(transform.position + transform.forward, range)) // Crée une sphère devant le joueur de radius range
               {
                   if (hitcol.gameObject.tag == "Enemy") // pour chaque ennemi dans la sphere
                   {
                       hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
                       Instantiate(stealLightFx, hitcol.transform.position, Quaternion.identity); // instantie le fx de vol de light
                   }
               }
           }
       }*/
    #endregion
    #endregion
    #region Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    // Gizmos.DrawWireSphere(transform.position + m_Position, m_Radius);       
    }
    #endregion
}
