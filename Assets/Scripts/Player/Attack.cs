using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject fxAttack;
    public int strength;

    void DoAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(fxAttack, transform.position + transform.forward, Quaternion.identity);
            foreach (Collider hitcol in Physics.OverlapSphere(transform.position+ transform.forward, 1f))
            {
                if (hitcol.gameObject.tag == "Enemy")
                {
                    hitcol.gameObject.GetComponent<EnemyLife>().LostLifePoint(strength);
                }
            }
        }
    }
    private void Update()
    {
        DoAttack();
    }
}
