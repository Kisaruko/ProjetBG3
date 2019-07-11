using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadGameScenes : MonoBehaviour
{

    public List<string> scenesGameToLoad;
    private IEnumerator coroutine;

    private bool loadScenes = false;
    public TextMeshProUGUI loadingText;
    public Slider loadingSlider;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        coroutine = LoadScenesAsync(scenesGameToLoad);
        StartCoroutine(coroutine);
    }

    IEnumerator LoadScenesAsync(List<string> scenesToLoad)
    {
        Scene originalScene = SceneManager.GetActiveScene();

        List<AsyncOperation> sceneLoads = new List<AsyncOperation>();

        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
            sceneLoading.allowSceneActivation = false;
            sceneLoads.Add(sceneLoading);
            while (sceneLoads[i].progress < 0.9f)
            {
                yield return null;
            }
        }

        for (int i = 0; i < sceneLoads.Count; i++)
        {
            sceneLoads[i].allowSceneActivation = true;
            while (!sceneLoads[i].isDone)
            {
                yield return null;
            }
        }

        AsyncOperation sceneUnloading = SceneManager.UnloadSceneAsync(originalScene);
        while (!sceneUnloading.isDone)
        {
            yield return null;
        }
    }
}
