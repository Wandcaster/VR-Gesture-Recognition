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
        get { return gestureData.tempGestureImage;}
        set { gestureData.tempGestureImage = value;}
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
        gestureData.InitGestureImage();
        gestureImage.filterMode = FilterMode.Point;
        this.rawPoints = rawPoints;
    }
    public void Save(string path)
    {
        DirectoryInfo info=Directory.CreateDirectory(path);
        Debug.Log(info.Exists);
        //AssetDatabase.CreateAsset(gestureData, path + ".asset");
        //SaveImage(path);
        //EditorUtility.FocusProjectWindow();
        //Selection.activeObject = gestureData;
    }
    private void SaveImage(string path)
    {
        byte[] bytes = ImageConversion.EncodeArrayToPNG(gestureImage.GetRawTextureData(), gestureImage.graphicsFormat, (uint)gestureImage.width, (uint)gestureImage.height);
        File.WriteAllBytes(path+"/"+gestureName+".png", bytes);
    }
}
