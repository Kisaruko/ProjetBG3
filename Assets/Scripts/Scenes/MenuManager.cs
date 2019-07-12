using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
public class MenuManager : MonoBehaviour
{

    public void StartGame()
    {
        Initiate.Fade("Loading", Color.black, 0.5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCredits()
    {
        Initiate.Fade("Credits", Color.black, 0.8f);
    }

    public void LoadOptions()
    {
        Initiate.Fade("Options", Color.black, 0.8f);
    }

    public void ReturnToMenu()
    {
        var go = GameObject.Find("FMOD.UnityIntegration.RuntimeManager");
        Destroy(go);
        Initiate.Fade("MainMenu", Color.black, 0.8f);
    }
}
