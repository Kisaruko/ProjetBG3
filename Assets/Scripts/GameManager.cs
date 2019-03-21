using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;
    private float myDeltaTime;
    private bool isTimeScaleAltered;
    public Material pencilMat;
    private Camera myCam;
    private ImageEffect imageeffect;
    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        myDeltaTime = Time.deltaTime;
        myCam = Camera.main;
        imageeffect = myCam.GetComponent<ImageEffect>();
    }

    public static void ShowAnImpact(float timeAlteredDuration)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ImpactEffect(timeAlteredDuration));
    }

    private IEnumerator ImpactEffect(float timeAlteredDuration)
    {
        //CameraShake.Shake(0.1f, 0.2f);
        Time.timeScale = 0.01f;
        imageeffect.enabled = true;
        isTimeScaleAltered = true;
        //yield return new WaitForSecondsRealtime(0.15f);
        yield return new WaitForSecondsRealtime(timeAlteredDuration);
        isTimeScaleAltered = false;
        Time.timeScale = 1f;
        //imageeffect.enabled = false;
        StopCoroutine("ImpactEffect");
    }
    private void Update()
    {
        if (isTimeScaleAltered)
        {         
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, myDeltaTime * 0.1f);
            pencilMat.SetFloat("_ColorThreshold", Mathf.MoveTowards(pencilMat.GetFloat("_ColorThreshold"), 1f, myDeltaTime *0.95f));
        }
        if(pencilMat.GetFloat("_ColorThreshold") !=0 && !isTimeScaleAltered)
        {
            pencilMat.SetFloat("_ColorThreshold", Mathf.MoveTowards(pencilMat.GetFloat("_ColorThreshold"), 0f, myDeltaTime * 0.95f));
        }
    }

}
