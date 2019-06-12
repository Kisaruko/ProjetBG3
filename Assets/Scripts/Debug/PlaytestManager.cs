using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaytestManager : MonoBehaviour
{
    public string sceneToLoad;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            SceneManager.LoadScene("PT_Sombre");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SceneManager.LoadScene("PT2_LightObjet");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SceneManager.LoadScene("PT3_Dash");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SceneManager.LoadScene("PT4_PlaquePression");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            SceneManager.LoadScene("PT5_PressionDash");
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            SceneManager.LoadScene("PT6_Lancer");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
