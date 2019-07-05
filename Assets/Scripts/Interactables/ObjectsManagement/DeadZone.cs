using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public List<string> activationTag;
    public bool mustDestroy = true;
    public GameObject vfxHit;
    private void OnTriggerEnter(Collider other)
    {
        if (mustDestroy)
        {
            //Compare if there is a tag in the List of tag
            foreach (string taggedTrigger in activationTag)
            {
                if (other.CompareTag(taggedTrigger) && other.GetComponentInParent<PlayerMovement>() != null)
                {
                    Destroy(other.gameObject);
                }
                else
                {
                    if (vfxHit != null)
                    {
                        Instantiate(vfxHit, other.transform.position, Quaternion.identity);
                    }
                    GameManager.ShowAnImpact(0.3f);
                    CameraShake.Shake(0.1f, 0.2f);
                    other.GetComponentInParent<PlayerMovement>().Recoil(transform, 3f);
                    other.GetComponentInParent<BinaryLight>().TakeHit();
                    Initiate.Fade("GameOver",Color.black, 0.8f);
                }
            }
        }
    }
}
