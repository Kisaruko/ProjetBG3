using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CorruptionBehaviour : MonoBehaviour
{
    [Header("Scale Variables")]

    public float maxScale;
    public float minScale;
    public float shrinkingTime;
    public float GrowingTime;
    public float rangeBeforeGrowing;

    public float purificationShrinkDivider= 1;

    [Header("VFX Variables")]
    public GameObject PurificationVfx;
    private bool hasStartGrowing;
    private Transform lightObject;

    private bool isPurified;
    private void Start()
    {
        lightObject = FindObjectOfType<LightDetection>().transform;
    }

    private void Update()
    {
        if (!isPurified)
        {
            if (Vector3.Distance(transform.position, lightObject.position) > rangeBeforeGrowing && hasStartGrowing == false)
            {
                Growing();
            }
        }
    }
    public void Shrinking()
    {
        transform.DOScale(minScale, shrinkingTime);
        this.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.black, shrinkingTime);
        hasStartGrowing = false;
    }

    public void Growing()
    {
        transform.DOScale(maxScale, GrowingTime);
        this.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.white, GrowingTime);

        hasStartGrowing = true;
    }

    public void Purification()
    {
        isPurified = true;
        hasStartGrowing = true;
        transform.DOScale(0.1f, shrinkingTime /purificationShrinkDivider);

        if (PurificationVfx != null)
        {
            Instantiate(PurificationVfx, transform.position,Quaternion.identity);
        }
        Destroy(this.gameObject, shrinkingTime);
    }
}

