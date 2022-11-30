using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "GestureDatabase", menuName = "GestureManager/GestureDatabase", order = 1)]
namespace VRGesureRecognition
{
    public abstract class IGestureDatabase : ScriptableObject
    {
        public string databaseName = "Database";
        public List<IGesture> gestures;
        public abstract void InitGestureDatabase();
    }
}
