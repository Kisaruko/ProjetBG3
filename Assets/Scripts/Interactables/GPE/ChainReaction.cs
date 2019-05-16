using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainReaction : MonoBehaviour
{
    private SwitchBehaviour switchbehaviour;
    public GameObject suckedLightVFX;
    public float range;
    public LayerMask switchs;
    private GameObject clone;
    private bool touchedSomething;
    private bool canRestartVfx = true;

    private void Start()
    {
        switchbehaviour = GetComponent<SwitchBehaviour>();
        if (switchbehaviour.isActivated)
        {
            DoChainReaction();
        }
    }
    public void DoChainReaction()
    {
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, switchs)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~switchs)) // si le ray ne touche pas de mur
            {
                if (hitcol.GetComponent<SwitchBehaviour>().isActivated == false)
                {
                    hitcol.GetComponent<SwitchBehaviour>().playerLight = this.gameObject;
                    hitcol.GetComponent<SwitchBehaviour>().Loading();

                    clone = Instantiate(suckedLightVFX, transform.position, Quaternion.identity);
                    clone.GetComponent<SuckedLightBehaviour>().light = transform;
                    clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = hitcol.transform;
                    clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                    canRestartVfx = false;
                    touchedSomething = true;
                }
                else
                {
                    //ouchedSomething = false;
                }
            }
            else
            {
                touchedSomething = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (switchbehaviour.isActivated && touchedSomething == true)
        {
            DoChainReaction();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
