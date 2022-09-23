using System;
using UnityEngine;

[Serializable]
public class Gesture 
{
    [SerializeField]
    private GestureData gestureData;
    public string gestureName
    {
        get { return gestureData.gestureName; }
        set { gestureData.gestureName = value; }
    }
    public int[,] points
    {
        get { return gestureData.points; }
        set { gestureData.points = value; gestureData.tempGestureImage = GetTexture2DFromPoints(value);}
    }
    public Texture2D gestureImage
    {
        get { return gestureData.tempGestureImage; }
        set { gestureData.tempGestureImage = value; gestureData.points = GetPointsFromTexture(value); }
    }

    public static int imageSize=200;
    public Gesture(string gestureName, int[,] points)
    {
        gestureData = ScriptableObject.CreateInstance("GestureData") as GestureData;
        this.gestureName = gestureName;
        this.points = points;
        gestureImage= GetTexture2DFromPoints(points);   
        gestureData.Init();
        gestureImage.filterMode = FilterMode.Point;
    }
    public Gesture(string gestureName, Texture2D gestureImage)
    {
        Init(gestureName, gestureImage);
    }
    public Texture2D GetTexture2DFromPoints(int[,] points)
    {
        Texture2D output= new Texture2D(imageSize,imageSize,gestureImage.format,false);
        Debug.Log(gestureImage.format);
        output.filterMode=FilterMode.Point;
        for (int i = 0; i < imageSize; i++)
        {
            for (int j = 0; j < imageSize; j++)
            {
                if (points[i, j] == 1)
                {
                    output.SetPixel(i, j, Color.white);
                }
                else
                {
                    output.SetPixel(i, j, Color.black);
                }
            }
        }
        output.Apply();
        return output;
    }
    public int[,] GetPointsFromTexture(Texture2D texture2D)
    {
        int[,] output=new int[imageSize, imageSize];

        for (int i = 0; i < imageSize; i++)
        {
            for (int j = 0; j < imageSize; j++)
            {
                if (texture2D.GetPixel(i, j).r == 0)
                {
                    output[i, j] = 1;
                }
            }
        }
        return output;
    }
    public void Init(string gestureName, Texture2D gestureImage)
    {
        gestureData = ScriptableObject.CreateInstance("GestureData") as GestureData;
        this.gestureName = gestureName;
        this.gestureImage = gestureImage;
        gestureData.InitGestureImage();
        gestureImage.filterMode = FilterMode.Point;
        points = GetPointsFromTexture(gestureImage);
    }
}
