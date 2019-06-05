using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public Image imageToFade;
    public float timeBeforeFade;
    public bool fadeIn;
    public float timeToFade;

    private void Start()
    {
        
    }

    IEnumerator FadeImage(Image imageToFade, bool fadeIn, float timeBeforeFade, float timeToFade)
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

        if (!imageToFade.enabled)
            imageToFade.enabled = true;

        Color textColor = imageToFade.color;

        while (counter < timeToFade)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / timeToFade);

            imageToFade.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        if (!fadeIn)
        {
            imageToFade.enabled = false;
        }
    }
}
