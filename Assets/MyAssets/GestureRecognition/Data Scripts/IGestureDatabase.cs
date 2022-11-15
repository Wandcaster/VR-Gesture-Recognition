using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "GestureDatabase", menuName = "GestureManager/GestureDatabase", order = 1)]
public abstract class IGestureDatabase:ScriptableObject
{
    public string databaseName = "Database";
    public List<IGesture> gestures = new List<IGesture>();
    public abstract void InitGestureDatabase();
}
