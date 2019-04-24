using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicTrigger : MonoBehaviour
{
    private PlayableDirector timeline;
    public float timeBeforeRecover; // temps avant que le player retrouve le control 
    public float timeBeforeRotate; // temps avant que la rotation de la cam
    public GameObject cam; // cam parent assigné
    private void Start()
    {
        timeline = GetComponent<PlayableDirector>(); // get le PlayableDirector pour lancer la timeline
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag ("Player"))
        {
            timeline.Play(); // lance la timeline d'anime et d'activation des game objects
            GameManager.RemoveControls(); // désactive les controls du player
            cam.GetComponent<CameraBehaviour>().followTarget = false; // get le script CameraBehaviour et désactive le follow target de la cam parent           
            StartCoroutine("Manager");
        }
    }
    private IEnumerator Manager()
    {
        yield return new WaitForSecondsRealtime(timeBeforeRotate); 
        cam.GetComponent<Cinematic>().isRotating = true; // get le script cinematic et active la rotation de la caméra

        yield return new WaitForSecondsRealtime(timeBeforeRecover);
        cam.GetComponent<Cinematic>().isRotating = false; // get le script cinematic et désactive la rotation
        cam.GetComponent<CameraBehaviour>().followTarget = true; // get le script CameraBehaviour et active le follow de la cam parent
        GameManager.RestoreControls(); // active les controls du player
        GetComponent<BoxCollider>().enabled = false; // désactive le trigger 
        StopCoroutine("Manager");


    }
}
