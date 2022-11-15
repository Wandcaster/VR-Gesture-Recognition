using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GestureVectorDatabase : IGestureDatabase
{
    // Start is called before the first frame update
    public override void InitGestureDatabase()
    {
        gestures.Clear();
        string path = AssetDatabase.GetAssetPath(this);
        path = path.Remove(path.Length - databaseName.Length - 6, databaseName.Length + 6);
        foreach (var item in Directory.GetDirectories(path))
        {
            path = Directory.GetFiles(item)[0].Remove(Directory.GetFiles(item)[0].Length - 6, 6);
            path = path.Remove(0, 17);
            gestures.Add(new VectorGesture(Resources.Load<VectorGestureData>(path)));
        }
    }
}
