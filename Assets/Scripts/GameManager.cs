using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;
    private GameObject player;
    private float myDeltaTime;
    private bool isTimeScaleAltered;
    public Material pencilMat;
    private Camera myCam;
    private CameraBehaviour camParent;
    private ImageEffect imageeffect;
    private PlayerMovement playermovement;
    private Attack attack;
    private PlayerShoot playershoot;
    private float baseGradTresh;
    private float baseOutlineTresh;
    public Color impactDrawColor;
    public Color impactBackGroundColor;
    public Color flashDrawColor;
    public Color flashBackGroundColor;

    void Awake()
    {
        _instance = this;
    }
    #region MainMethods
    private void Start()
    {
        myDeltaTime = 0.03f;
        myCam = Camera.main;
        imageeffect = myCam.GetComponent<ImageEffect>();
        camParent = myCam.GetComponentInParent<CameraBehaviour>();
        pencilMat.SetFloat("_GradThresh", 0.2f);
        pencilMat.SetFloat("_OutLineTresh",20f);
        baseGradTresh = pencilMat.GetFloat("_GradThresh");
        baseOutlineTresh = pencilMat.GetFloat("_OutLineTresh");
        player = GameObject.Find("Player");
        playermovement = player.GetComponent<PlayerMovement>();
        attack = player.GetComponentInChildren<Attack>();
        playershoot = player.GetComponent<PlayerShoot>();
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
    #endregion
    #region ImpactEffect
    public static void ShowAnImpact(float timeAlteredDuration)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ImpactEffect(timeAlteredDuration));
    }
    private IEnumerator ImpactEffect(float timeAlteredDuration)
    {
        pencilMat.SetColor("_DrawingColor", impactDrawColor);
        pencilMat.SetColor("_BackGroundColor", impactBackGroundColor);
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
    public static void ShowAZoom(float duration)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ZoomOnAction(duration));
    }
    private IEnumerator ZoomOnAction(float duration)
    {
        Time.timeScale = 0.01f;
        isTimeScaleAltered = true;
        //camParent.camHeight = -5f;
        //camParent.camDistance = 0f;
        yield return new WaitForSecondsRealtime(duration);
        //camParent.camDistance = 6f;
        //camParent.camHeight = 10f;
        isTimeScaleAltered = false;
        Time.timeScale = 1f;
        
        
        StopCoroutine("ZoomOnAction");
    }

    #endregion
    #region Controls
    public static void RemoveControls()
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ControlsRemoved());
    }
    private IEnumerator ControlsRemoved()
    {
       playermovement.controlsAreEnabled = false;
       attack.AuthorizedToAttack = false;
       playershoot.AuthorizedToShoot = false;
       StopCoroutine("ControlsRemoved");
       yield return null;
    }
    public static void RestoreControls()
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.ControlsRestored());
    }
    private IEnumerator ControlsRestored()
    {
        playermovement.controlsAreEnabled = true;
        attack.AuthorizedToAttack = true;
        playershoot.AuthorizedToShoot = true;
        StopCoroutine("ControlsRestored");
        yield return null;
    }

    #endregion
    #region Flash
    /*public static void Flash(float timeAlteredDuration)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.FlashScreen(timeAlteredDuration));
    }
    private IEnumerator FlashScreen(float timeAlteredDuration)
    {
        pencilMat.SetColor("_DrawingColor", flashDrawColor);
        pencilMat.SetColor("_BackGroundColor", flashBackGroundColor);
        imageeffect.isFiltered = true;
        yield return new WaitForSecondsRealtime(timeAlteredDuration);
        imageeffect.isFiltered = false;
        StopCoroutine("FlashScreen");
    }*/
    #endregion
}
