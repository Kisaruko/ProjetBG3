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
    private Transform actualVfxTarget;
    private int frames;

    private int loadMultiplier = 10;

    public List<SwitchBehaviour> GetSwitchInRange()
    {
        List<SwitchBehaviour> switchsList = new List<SwitchBehaviour>(); //crée une liste

        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, switchs)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~switchs)) // si le ray ne touche pas de mur
            {
                if (hitcol.GetComponent<SwitchBehaviour>() != null && hitcol.GetComponent<SwitchBehaviour>().isActivated == false)
                {
                    SwitchBehaviour switchbehaviour = hitcol.GetComponent<SwitchBehaviour>();

                    if (switchbehaviour != null) // si l'ennemi a le composant enemy life
                    {
                        switchsList.Add(switchbehaviour);// add un composant a la liste
                        hitcol.GetComponent<SwitchBehaviour>().playerLight = this.gameObject;
                        for (int i = 0; i < loadMultiplier; i++)
                        {
                            hitcol.GetComponent<SwitchBehaviour>().Loading();
                        }
                        int index = Random.Range(0, switchsList.Count);
                        actualVfxTarget = switchsList[index].transform;

                    }
                }
                if(hitcol.GetComponent<EmitWhenTrigger>() != null)
                {
                    hitcol.GetComponent<EmitWhenTrigger>().ActivateEmission();
                }
            }
        }
        return switchsList;
    }

    private void FixedUpdate()
    {
        frames++;
        if (GetComponent<SwitchBehaviour>().isActivated && frames % 1 == 0)
        {
            List<SwitchBehaviour> touchedSwitchs = GetSwitchInRange();
            foreach (SwitchBehaviour switchBehaviour in touchedSwitchs)
            {
                clone = Instantiate(suckedLightVFX, transform.position, Quaternion.identity);
                clone.GetComponent<SuckedLightBehaviour>().light = transform;
                clone.GetComponent<SuckedLightBehaviour>().isSucked = true;
                clone.GetComponent<SuckedLightBehaviour>().mobSuckingSpot = actualVfxTarget;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
