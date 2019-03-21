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
    private float baseGradTresh;
    private float baseOutlineTresh;

    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        myDeltaTime = 0.03f;
        myCam = Camera.main;
        imageeffect = myCam.GetComponent<ImageEffect>();

        pencilMat.SetFloat("_GradThresh", 0.2f);
        pencilMat.SetFloat("_OutLineTresh",20f);
        baseGradTresh = pencilMat.GetFloat("_GradThresh");
        baseOutlineTresh = pencilMat.GetFloat("_OutLineTresh");
    }
    private void Update()
    {
        #region lerping Shader
        if (isTimeScaleAltered)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, myDeltaTime * 0.1f);
            pencilMat.SetFloat("_ColorThreshold", Mathf.MoveTowards(pencilMat.GetFloat("_ColorThreshold"), 1f, myDeltaTime * 6.27f));
            pencilMat.SetFloat("_GradThresh", Mathf.MoveTowards(pencilMat.GetFloat("_GradThresh"), 0.03f, myDeltaTime * 1.98f));
            pencilMat.SetFloat("_OutLineTresh", Mathf.MoveTowards(pencilMat.GetFloat("_OutLineTresh"), 3f, myDeltaTime * 198));

        }
        if (pencilMat.GetFloat("_ColorThreshold") != 0 && !isTimeScaleAltered)
        {
            pencilMat.SetFloat("_ColorThreshold", Mathf.MoveTowards(pencilMat.GetFloat("_ColorThreshold"), 0f, myDeltaTime * 0.95f));
            pencilMat.SetFloat("_GradThresh", Mathf.MoveTowards(pencilMat.GetFloat("_GradThresh"), baseGradTresh, myDeltaTime * 0.3f));
            pencilMat.SetFloat("_OutLineTresh", Mathf.MoveTowards(pencilMat.GetFloat("_OutLineTresh"), baseOutlineTresh, myDeltaTime * 30));
        }
        #endregion
    }

    #region ImpactEffect
    public static void ShowAnImpact(float timeAlteredDuration)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ImpactEffect(timeAlteredDuration));
    }

    private IEnumerator ImpactEffect(float timeAlteredDuration)
    {
        Time.timeScale = 0.01f;
        imageeffect.isFiltered = true;
        isTimeScaleAltered = true;
        yield return new WaitForSecondsRealtime(timeAlteredDuration);
        isTimeScaleAltered = false;
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.3f);
        imageeffect.isFiltered = false;

        StopCoroutine("ImpactEffect");
    }
    #endregion


}
