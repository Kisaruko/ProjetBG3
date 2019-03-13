using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Variables
    [Header("Values", order = 0)]
    [Space(10, order = 1)]
    public int strengthAttack1;
    public int strengthAttack2;
    public int strengthAttack3;
    public int multiplierLightRegenAttack3;
    public float speedWhileAttacking;

    [Header("Detection", order = 0)]
    [Space(10, order = 1)]
    [Range(0, 180)]
    public float angle;
    public float range;
    public LayerMask attackSphereDetection;

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
    float maxComboDelay = 0.5f;

    #endregion

    #region Main Methods
    void Start()
    {
        anim = GetComponentInChildren<Animator>(); // get L'animator
        playermovement = GetComponentInParent<PlayerMovement>(); // get le mouvement
        anim.SetBool("Attack1", false); //On set toutes les variables d'animation a false par précaution
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            OnButtonClick();
        }
        if (Time.time - lastClickedTime > maxComboDelay) // si le joueur n'a pas appuyé de maniere répétée assez vite
        {
            noOfClicks = 0; // reinitialise le combo
            anim.SetBool("Attack1", false); //Reset toutes les variables d'anim a false
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack3", false);
        }
        if (noOfClicks != 0)
        {
            playermovement.moveSpeed = speedWhileAttacking; // le player ralenti
        }
    }
    #endregion
    #region Custom Methods

    #region InputDetection
    void OnButtonClick()
    {
        //Record time of last button click
        lastClickedTime = Time.time; // get le moment ou j'ai appuyé
        noOfClicks++;//add 1click
        if (noOfClicks == 1)
        {
            anim.SetBool("Attack1", true);
        }
        //limit/clamp no of clicks between 0 and 3 because you have combo for 3 clicks
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
    }

    public void ComboCheck1()
    {
        anim.SetBool("Attack1", false);
        if (noOfClicks >= 2)
        {
            anim.SetBool("Attack2", true);

        }
        else
        {
            noOfClicks = 0;
        }
    }// Check si le player a réappuyé
    public void ComboCheck2()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        if (noOfClicks >= 3)
        {
            anim.SetBool("Attack3", true);
            noOfClicks = 0;

        }
    }// Check si le player a réappuyé
    #endregion

    #region AttackBehaviour 

    private List<EnemyLife> DetectEnemiesInRange()
    {
        List<EnemyLife> enemies = new List<EnemyLife>(); //crée une liste
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, attackSphereDetection)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~attackSphereDetection)) // si le ray touche l'ennemi
            {
                float angleToCollider = Vector3.Angle(transform.forward, toCollider.normalized); //check l'angle entre le player et l'ennemi
                if (angleToCollider <= angle / 2) // si l'angle est inférieur à la range du joueur
                {
                    EnemyLife enemyLife = hitcol.GetComponent<EnemyLife>(); 
                    if (enemyLife != null) // si l'ennemi a le composant enemy life
                    {
                        enemies.Add(enemyLife);// add un composant a la liste
                    }
                }
            }
        }
        return enemies;// retourne la liste
    }

    public void Attack1()
    {
        Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity);
        List<EnemyLife> touchedEnemies = DetectEnemiesInRange();
        foreach (EnemyLife enemyLife in touchedEnemies)
        {
            enemyLife.LostLifePoint(strengthAttack1); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
            Instantiate(stealLightFx, enemyLife.transform.position, Quaternion.identity); // instantie le fx de vol de light
        }
    }

    public void Attack2()
    {
        Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity);
        List<EnemyLife> touchedEnemies = DetectEnemiesInRange();
        foreach (EnemyLife enemyLife in touchedEnemies)
        {
            enemyLife.LostLifePoint(strengthAttack2); // appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
            Instantiate(stealLightFx, enemyLife.transform.position, Quaternion.identity); // instantie le fx de vol de light
        }
    }

    public void Attack3()
    {
        Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity); // Instantier le fx d'attaque
        List<EnemyLife> touchedEnemies = DetectEnemiesInRange();
        foreach (EnemyLife enemyLife in touchedEnemies)
        {
            enemyLife.LostLifePoint(strengthAttack2);  //appelle la fonction de perte de pdv du monstre, les dégats infligés sont égaux a strength
            enemyLife.gameObject.GetComponent<RecoilEnemy>().StartCoroutine("RecoilTime");
            for (int i = 0; i < multiplierLightRegenAttack3; i++) // répéter nbMultiplierlig... de fois l'action
            {
                Instantiate(stealLightFx, enemyLife.transform.position, Quaternion.identity); // instantie le fx de vol de light
            }
            CameraShake.Shake(0.2f, 2f);
        }
    }
    #endregion
    #endregion
    #region Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 startPos = transform.position;
        Vector3 endPos = Vector3.zero;
        Vector3 direction = transform.forward * range;
        Quaternion rotation = Quaternion.AngleAxis(angle / 2, Vector3.up);
        endPos = startPos + rotation * direction;

        Gizmos.DrawLine(startPos, endPos);
        rotation = Quaternion.AngleAxis(-angle / 2, Vector3.up);
        endPos = startPos + rotation * direction;
        Gizmos.DrawLine(startPos, endPos);

    }
    #endregion
}
