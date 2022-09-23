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

        if (GUILayout.Button("Start Recording"))
        {
            myScript.StartRecording();
        }
        if (GUILayout.Button("Stop Recording"))
        {
            myScript.StopRecording();
        }
        if(GUILayout.Button("Draw Gesture"))
        {
            myScript.DrawGesture();
        }
        if(GUILayout.Button("Save draw to File"))
        {
            myScript.SaveGesture();
        }
    }
}
