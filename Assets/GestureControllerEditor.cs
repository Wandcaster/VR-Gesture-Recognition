using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GestureController))]
public class GestureControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GestureController myScript = (GestureController)target;

        if (GUILayout.Button("StartRecording"))
        {
            myScript.StartCoroutine(myScript.StartRecording());
        }
        if (GUILayout.Button("StopRecording"))
        {
            myScript.StopRecording();
        }
    }
}
