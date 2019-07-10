using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    public TextMeshProUGUI textToFade;
    public float timeBeforeFade;
    public bool fadeIn;
    public float timeToFade;

    void Start()
    {
        StartCoroutine(FadeText(textToFade, fadeIn, timeBeforeFade, timeToFade));
    }

    IEnumerator FadeText(TextMeshProUGUI textToFade, bool fadeIn, float timeBeforeFade, float timeToFade)
    {
        yield return new WaitForSeconds(timeBeforeFade);

        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = 1;
            b = 0;
        }

        if (!textToFade.enabled)
            textToFade.enabled = true;

        Color textColor = textToFade.color;
        
        while(counter < timeToFade)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / timeToFade);

            textToFade.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
        
        if(!fadeIn)
        {
            textToFade.enabled = false;
        }

    }
}
