using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour
{
    private Vector3 fxPosition;

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject)
        {
            fxPosition = EventSystem.current.currentSelectedGameObject.transform.position;
            Debug.Log(fxPosition);
        }
        
    }
}
