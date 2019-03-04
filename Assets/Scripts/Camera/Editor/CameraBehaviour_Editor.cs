using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraBehaviour))]
public class CameraBehaviour_Editor : Editor
{
    #region Variables
    private CameraBehaviour targetCamera;
    #endregion

    #region Main Methods
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnEnable()
    {
        targetCamera = (CameraBehaviour)target;
    }
    private void OnSceneGUI()
    {
        //Make sure we have a target first
        if(!targetCamera.target)
        {
            return;
        }

        //Storing target reference
        Transform camTarget = targetCamera.target;

        //Draw distance disc
        Handles.color = new Color(1f, 0f, 0f, 0.15f);
        Handles.DrawSolidDisc(targetCamera.target.position, Vector3.up, targetCamera.camDistance);

        Handles.color = new Color(1f, 1f, 0f, 0.75f);
        Handles.DrawWireDisc(targetCamera.target.position, Vector3.up, targetCamera.camDistance);

        //Create Slider handle to adjust camera propreties
        Handles.color = new Color(1f, 0f, 0f, 0.5f);
        targetCamera.camDistance = Handles.ScaleSlider(targetCamera.camDistance, camTarget.position, -camTarget.forward, Quaternion.identity, targetCamera.camDistance, 1f);
        targetCamera.camDistance = Mathf.Clamp(targetCamera.camDistance, 2f, float.MaxValue);

        Handles.color = new Color(0f, 0f, 1f, 0.5f);
        targetCamera.camHeight = Handles.ScaleSlider(targetCamera.camHeight, camTarget.position, Vector3.up, Quaternion.identity, targetCamera.camHeight, 1f);
        targetCamera.camHeight = Mathf.Clamp(targetCamera.camHeight, 2f, float.MaxValue);

        //Create labels & style
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(camTarget.position + (-camTarget.forward * targetCamera.camDistance), "Distance", labelStyle);
        Handles.Label(camTarget.position + (Vector3.up * targetCamera.camHeight), "Height", labelStyle);
    }
    #endregion
}
