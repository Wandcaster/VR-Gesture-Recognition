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
        get { return gestureData.points;}
        set { gestureData.points = value;}
    }
    public Texture2D gestureImage
    {
        get { return gestureData.tempGestureImage;}
        set { gestureData.tempGestureImage = value;}
    }
    public Gesture(string gestureName, Texture2D gestureImage)
    {
        Init(gestureName, gestureImage);
    } 
    public int[,] GetPointsFromTexture(Texture2D texture2D)
    {
        int[,] output=new int[texture2D.width, texture2D.height];

        for (int i = 0; i < texture2D.width; i++)
        {
            for (int j = 0; j < texture2D.height; j++)
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
