using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleAI))]
public class SimpleAIEditor : Editor
{
    private void OnSceneGUI()
    {
        SimpleAI simpleAI = (SimpleAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(simpleAI.transform.position, Vector3.up, Vector3.forward, 360, simpleAI.viewRadius);
        Vector3 viewAngleA = simpleAI.DirFromAngle(-simpleAI.viewAngle / 2, false);
        Vector3 viewAngleB = simpleAI.DirFromAngle(simpleAI.viewAngle / 2, false);

        Handles.DrawLine(simpleAI.transform.position, simpleAI.transform.position + viewAngleA * simpleAI.viewRadius);
        Handles.DrawLine(simpleAI.transform.position, simpleAI.transform.position + viewAngleB * simpleAI.viewRadius);

        Handles.color = Color.blue;
        Handles.DrawWireArc(simpleAI.transform.position, Vector3.up, Vector3.forward, 360, simpleAI.wanderRadius);

        Handles.color = Color.red;
        Handles.DrawWireArc(simpleAI.transform.position, Vector3.up, Vector3.forward, 360, simpleAI.absorbRange);
    }
}
