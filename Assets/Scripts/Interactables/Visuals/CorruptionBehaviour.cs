using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CorruptionBehaviour : MonoBehaviour
{

    public float shrinkingTime;
    public float GrowingTime;
    public float rangeBeforeGrowing;

    public float purificationShrinkDivider = 1;

    [Header("VFX Variables")]
    public GameObject PurificationVfx;
    private bool hasStartGrowing;
    private Transform lightObject;
    public float timeToDissolve;

    [Header("Scrolling Variables")]
    public float newScrollX;
    public float newScrollY;

    private bool isPurified;
    private ScrollingTexture scrollingtexture;
    private float baseScrollX;
    private float baseScrollY;

    private void Start()
    {
        lightObject = FindObjectOfType<LightDetection>().transform;
        if (GetComponent<ScrollingTexture>() != null)
        {
            scrollingtexture = GetComponent<ScrollingTexture>();
            baseScrollX = scrollingtexture.scrollXSpeed;
            baseScrollY = scrollingtexture.scrollYSpeed;
        }
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
        // transform.DOScale(minScale, shrinkingTime);
        if (scrollingtexture != null)
        {
            DOTween.To(() => scrollingtexture.scrollXSpeed, x => scrollingtexture.scrollXSpeed = x, newScrollX, shrinkingTime);
            DOTween.To(() => scrollingtexture.scrollYSpeed, x => scrollingtexture.scrollYSpeed = x, newScrollY, shrinkingTime);
            scrollingtexture.SetSpeed();
        }
        this.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.black, shrinkingTime);
        hasStartGrowing = false;
    }

    public void Growing()
    {
        //  transform.DOScale(maxScale, GrowingTime);
        if (scrollingtexture != null)
        {
            DOTween.To(() => scrollingtexture.scrollXSpeed, x => scrollingtexture.scrollXSpeed = x, baseScrollX, GrowingTime);
            DOTween.To(() => scrollingtexture.scrollYSpeed, x => scrollingtexture.scrollYSpeed = x, baseScrollY, GrowingTime);
            scrollingtexture.SetSpeed();
        }
        this.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.white, GrowingTime);

        hasStartGrowing = true;
    }

    public void Purification()
    {
        if (PurificationVfx != null)
        {
            if(GetComponent<ToggleParticleManager>() != null)
            {
                GetComponent<ToggleParticleManager>().Off();
            }
            Invoke("StartPurification", Random.Range(0.1f, 1f));
        }
    }

    private void StartPurification()
    {
        Instantiate(PurificationVfx, transform.position, Quaternion.identity);
        isPurified = true;
        hasStartGrowing = true;
        GetComponent<MeshRenderer>().material.DOFloat(1, "_Amount", timeToDissolve);
        Destroy(this.gameObject, shrinkingTime);
    }
    private void OnDestroy()
    {
        if (GetComponent<ToggleParticleManager>() != null)
        {
            CameraShake.Shake(0.5f, 1f);
        }
    }
}

