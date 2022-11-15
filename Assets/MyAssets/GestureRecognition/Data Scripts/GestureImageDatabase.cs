using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GestureImageDatabase : IGestureDatabase
{
    public override void InitGestureDatabase()
    {
        gestures.Clear();
        string path = AssetDatabase.GetAssetPath(this);
        path = path.Remove(path.Length - databaseName.Length - 6, databaseName.Length + 6);
        foreach (var item in Directory.GetDirectories(path))
        {
            path = Directory.GetFiles(item)[0].Remove(Directory.GetFiles(item)[0].Length - 6, 6);
            path = path.Remove(0, 17);
            gestures.Add(new ImageGesture(Resources.Load<ImageGestureData>(path)));
        }
    }
}
