using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GestureDatabase))]
public class GestureDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GestureDatabase myScript = (GestureDatabase)target;
        if (GUILayout.Button("Load all gesture to list"))
        {
            myScript.InitGestureDatabase();
        }
    }

    
}
