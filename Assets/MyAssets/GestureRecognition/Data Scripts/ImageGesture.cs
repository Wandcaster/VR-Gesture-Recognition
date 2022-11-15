using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ImageGesture:IGesture
{
    [SerializeField]
    private ImageGestureData imageGestureData
    {
        get
        {
            return (ImageGestureData)gestureData;
        }
        set
        {
            gestureData = value;
        }
    }
    public List<Vector2> rawPoints
    {
        get { return imageGestureData.points;}
        set { imageGestureData.points = value;}
    }
    public Texture2D gestureImage
    {
        get { return imageGestureData.gestureImage;}
        set { imageGestureData.gestureImage = value;}
    }
    public override void Save(string path)
    {
        path += "/" + gestureName;
        Directory.CreateDirectory(path);
        SaveImage(path);
        ImageGestureData temp = ScriptableObject.CreateInstance<ImageGestureData>();
        EditorUtility.CopySerialized(imageGestureData, temp);
        AssetDatabase.CreateAsset(temp, path + "/" + gestureName + ".asset");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = imageGestureData;
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
    public ImageGesture(ImageGestureData gestureData):base(gestureData)
    {
        this.imageGestureData = gestureData;
    }
    public ImageGesture(string gestureName, Texture2D gestureImage, List<Vector2> rawPoints) : base(null)
    {
        imageGestureData = ScriptableObject.CreateInstance< ImageGestureData>();
        this.gestureName = gestureName;
        this.gestureImage = gestureImage;
        gestureImage.filterMode = FilterMode.Point;
        this.rawPoints = rawPoints;
    }
}
