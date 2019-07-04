using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Initiate.Fade("MainMenu", Color.black, 0.8f);
    }
}
