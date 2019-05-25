using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateController))]
public class FSMEditor : Editor
{
    private void OnSceneGUI()
    {
        StateController controller = (StateController)target;
        Handles.color = controller.currentState.sceneGizmoColor;
        Handles.DrawWireArc(controller.transform.position, Vector3.up, Vector3.forward, 360, controller.trashMobStats.lookRange);
        Vector3 viewAngleA = DirFromAngle(-controller.trashMobStats.lookAngle / 2, false, controller);
        Vector3 viewAngleB = DirFromAngle(controller.trashMobStats.lookAngle / 2, false, controller);

        Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngleA * controller.trashMobStats.lookRange);
        Handles.DrawLine(controller.transform.position, controller.transform.position + viewAngleB * controller.trashMobStats.lookRange);

        Handles.color = controller.currentState.sceneGizmoColor - new Color(0f, 0f, 0f, 0.95f);
        Handles.DrawSolidDisc(controller.transform.position, Vector3.up, controller.trashMobStats.lookRange);

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(controller.transform.position, controller.currentState.ToString(), labelStyle);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, StateController controller)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += controller.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
