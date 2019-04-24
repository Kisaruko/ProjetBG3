using UnityEngine;
using System.Collections;

public class CycleMoving : MonoBehaviour
{
    public float speed = 2;
    public GameObject touchedFx;

    private void Start()
    {
        InvokeRepeating("RevertSpeed", 1.5f, 3f);
    }
    private void Update()
    {
        transform.Translate((Vector3.forward * speed)*Time.deltaTime);
    }
    void RevertSpeed()
    {
        speed = speed * -1;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Success");
        Instantiate(touchedFx, transform.position, Quaternion.identity);
    }

}
