using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureDatabase", menuName = "GestureManager/GestureDatabase", order = 1)]
public class GestureDatabase : ScriptableObject
{
    public string databaseName="Database";
    public List<Gesture> gestures=new List<Gesture>();
}
