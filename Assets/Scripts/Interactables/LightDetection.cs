using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float range;
    public LayerMask mask;
    private Light light;
    public float rangeMultiplier;
    public ParticleSystem loadSwitch;
    private void Start()
    {
        light = GetComponentInChildren<Light>();
    }
    private void Update()
    {
        range = light.intensity * rangeMultiplier;
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, mask)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~mask)) // si le ray ne touche pas de mur
            {
                hitcol.GetComponent<SwitchBehaviour>().playerLight = this.gameObject;
                hitcol.GetComponent<SwitchBehaviour>().Loading();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
