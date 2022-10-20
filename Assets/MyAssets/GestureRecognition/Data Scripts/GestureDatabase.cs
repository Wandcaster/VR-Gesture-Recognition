using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureDatabase", menuName = "GestureManager/GestureDatabase", order = 1)]
public class GestureDatabase : ScriptableObject
{
    public string databaseName = "Database";
    public List<Gesture> gestures = new List<Gesture>();
    public  void InitGestureDatabase()
    {
        gestures.Clear();
        string path = AssetDatabase.GetAssetPath(this);
        path = path.Remove(path.Length - databaseName.Length - 6, databaseName.Length + 6);
        foreach (var item in Directory.GetDirectories(path))
        {
            path = Directory.GetFiles(item)[0].Remove(Directory.GetFiles(item)[0].Length - 6, 6);
            path = path.Remove(0, 17);
            gestures.Add(new Gesture(Resources.Load<GestureData>(path)));
        }
    }
}
