using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Gesture 
{
    [SerializeField]
    private GestureData gestureData;
    public int gestureID
    {
        get { return gestureData.ID; }
        set { gestureData.ID = value; }
    }
    public string gestureName
    {
        get { return gestureData.gestureName; }
        set { gestureData.gestureName = value; }
    }
    public List<Vector2> rawPoints
    {
        get { return gestureData.points;}
        set { gestureData.points = value;}
    }
    public Texture2D gestureImage
    {
        get { return gestureData.gestureImage;}
        set { gestureData.gestureImage = value;}
    }
    public Gesture(string gestureName, Texture2D gestureImage, List<Vector2> rawPoints)
    {
        Init(gestureName, gestureImage,rawPoints);
    } 
    public void Init(string gestureName, Texture2D gestureImage, List<Vector2> rawPoints)
    {
        gestureData = ScriptableObject.CreateInstance("GestureData") as GestureData;
        this.gestureName = gestureName;
        this.gestureImage = gestureImage;
        gestureImage.filterMode = FilterMode.Point;
        this.rawPoints = rawPoints;
    }
    public void Save(string path)
    {
        path += "/" + gestureName;
        Directory.CreateDirectory(path);
        SaveImage(path);
        if (AssetDatabase.Contains(gestureData))
        {
            GestureData temp = ScriptableObject.CreateInstance<GestureData>();
            EditorUtility.CopySerialized(gestureData, temp);
            AssetDatabase.CreateAsset(temp, path + "/" + gestureName + ".asset");
        }
        else
        {
            GestureData temp = ScriptableObject.CreateInstance<GestureData>();
            EditorUtility.CopySerialized(gestureData, temp);
            gestureData = temp;
            AssetDatabase.CreateAsset(temp, path + "/" + gestureName + ".asset");
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = gestureData;
    }
    private void SaveImage(string path)
    {
        byte[] bytes = ImageConversion.EncodeArrayToPNG(gestureImage.GetRawTextureData(), gestureImage.graphicsFormat, (uint)gestureImage.width, (uint)gestureImage.height);
        path = path + "/" + gestureName + ".png";
        File.WriteAllBytes(path, bytes);
        Debug.Log(path);
        AssetDatabase.Refresh();
        TextureImporter textureImporter = (TextureImporter)TextureImporter.GetAtPath(path);
        Debug.Log(textureImporter);
        textureImporter.isReadable = true;
        textureImporter.npotScale = TextureImporterNPOTScale.None;
        textureImporter.SaveAndReimport();
        path = path.Remove(0, 17);
        path = path.Remove(path.Length - 4, 4);
        gestureImage = Resources.Load<Texture2D>(path);
        Debug.Log(path);
    }
}
