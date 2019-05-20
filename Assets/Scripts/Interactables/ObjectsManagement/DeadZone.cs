using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public List<string> activationTag;
    public bool mustDestroy = true;
    private void OnTriggerEnter(Collider other)
    {
        if (mustDestroy)
        {
            //Compare if there is a tag in the List of tag
            foreach (string taggedTrigger in activationTag)
            {
                if (other.CompareTag(taggedTrigger))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
