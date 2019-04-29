using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    [SerializeField] float range;
    public LayerMask mask;
    private Light light;
    public float multiplier;
    private void Start()
    {
        light = GetComponentInChildren<Light>();
    }
    private void Update()
    {
        range = light.intensity*multiplier;
        foreach (Collider hitcol in Physics.OverlapSphere(transform.position, range, mask)) // crée une sphere de detection
        {
            Vector3 toCollider = hitcol.transform.position - transform.position; // get le vecteur entre ennemi et player
            Ray ray = new Ray(transform.position, toCollider); // trace un rayon entre les deux
            if (!Physics.Raycast(ray, toCollider.magnitude, ~mask)) // si le ray ne touche pas de mur
            {
                Debug.Log(hitcol.name);
                hitcol.GetComponent<Light>().intensity = Mathf.Lerp(0.5f,5f,3);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
