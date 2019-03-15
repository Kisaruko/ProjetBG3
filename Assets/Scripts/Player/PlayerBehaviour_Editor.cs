using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerBehaviour))]
public class PlayerBehaviour_Editor : Editor
{
    private PlayerBehaviour playerBehaviour;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnEnable()
    {
        playerBehaviour = (PlayerBehaviour)target;
    }

    private void OnSceneGUI()
    {
        Handles.color = new Color(1f, 0f, 0f, .75f);
        Handles.DrawWireDisc(playerBehaviour.transform.position, Vector3.up, playerBehaviour.shortLight.range);

        Handles.color = new Color(0f, 0f, 1f, .75f);
        Handles.DrawWireDisc(playerBehaviour.transform.position, Vector3.up, playerBehaviour.midLight.range);

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(new Vector3(playerBehaviour.transform.position.x, playerBehaviour.transform.position.y, playerBehaviour.shortLight.range),"Short Light Range", labelStyle);
        Handles.Label(new Vector3(playerBehaviour.transform.position.x, playerBehaviour.transform.position.y, playerBehaviour.midLight.range),"Mid Light Range", labelStyle);

    }
}
