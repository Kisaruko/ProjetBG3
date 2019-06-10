using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene(GameManager._instance.lastScene);
    }

    public void ReturnToDesktop()
    {
        Application.Quit();
        Debug.Log("g kité");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(GameManager._instance.menuSceneName);
    }
}
