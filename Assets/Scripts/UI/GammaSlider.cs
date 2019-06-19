using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GammaSlider : MonoBehaviour
{
    public float GammaCorrection;
    public float GainCorrection;
    public PostProcessProfile m_profile;

    ColorGrading m_colorGrading;

    public Rect GammaSliderLocation;
    public Rect GainSliderLocation;

    private void Start()
    {
        m_colorGrading = m_profile.GetSetting<ColorGrading>();
    }

    private void Update()
    {
        m_colorGrading.gamma.value = new Vector4(m_colorGrading.gamma.value.x, m_colorGrading.gamma.value.y, m_colorGrading.gamma.value.z, GammaCorrection);
        m_colorGrading.gain.value = new Vector4(m_colorGrading.gain.value.x, m_colorGrading.gain.value.y, m_colorGrading.gain.value.z, GainCorrection);
    }

    void OnGUI()
    {

        GammaCorrection = GUI.HorizontalSlider(GammaSliderLocation, GammaCorrection, 0, 1.0f);
        GainCorrection = GUI.HorizontalSlider(GainSliderLocation, GainCorrection, -1.0f, 1.0f);

    }
}