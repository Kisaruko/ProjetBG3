using UnityEngine;
using System.Collections;

public class MovingPlatformWithTime : MonoBehaviour
{
    public float speed = 2;
    public GameObject touchedFx;
    public GameObject player;
    private void Start()
    {
        InvokeRepeating("RevertSpeed", 0.5f, .5f);
    }
    private void Update()
    {
        transform.Translate((Vector3.forward * speed) * Time.deltaTime);
    }
    void RevertSpeed()
    {
        speed = speed * -1;
    }
    private void OnTriggerEnter(Collider other)
    {
       // Instantiate(touchedFx, transform.position, Quaternion.identity);
        //player.transform.position = transform.position;
        if (other.gameObject.layer == 15)
        {
            other.gameObject.transform.parent = this.transform;
        }
    }

}
