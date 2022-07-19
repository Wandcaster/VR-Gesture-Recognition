using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Gesture :MonoBehaviour
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
        set { gestureData.points = value; }
    }
    public Texture2D gestureImage
    {
        get { return gestureData.gestureImage; }
        set { gestureData.gestureImage = value; }
    }
    public int pointsCount
    {
        get { return gestureData.pointsCount; }
        set { gestureData.pointsCount = value; }
    }

    public static int imageSize=8;
    public Gesture(string gestureName, int[,] points)
    {
        this.gestureName = gestureName;
        this.points = points;
        gestureImage= CreateTexture2D();
        pointsCount = CalculatePointNumber();
    }

    public Texture2D CreateTexture2D()
    {
        Texture2D output = new Texture2D(imageSize, imageSize);
        output.filterMode=FilterMode.Point;
        for (int i = 0; i < imageSize; i++)
        {
            for (int j = 0; j < imageSize; j++)
            {
                if (points[i, j] == 1)
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
        int[,] output=new int[texture2D.width, texture2D.height];

        for (int i = 0; i < texture2D.width; i++)
        {
            for (int j = 0; j < texture2D.height; j++)
            {
                if (texture2D.GetPixel(i, j).r == 1)
                {
                    Debug.Log("black");
                    output[i, j] = 1;
                }
            }
        }
        return output;
    }

    public void GetPointsFromTexture()
    {
        int[,] output = new int[gestureImage.width, gestureImage.height];

        for (int i = 0; i < gestureImage.width; i++)
        {
            for (int j = 0; j < gestureImage.height; j++)
            {
                if (gestureImage.GetPixel(i, j).r == 1)
                {
                    Debug.Log("black");
                    output[i, j] = 1;
                }
            }
        }
        points = output;
    }   
    private void Start()
    {
        gestureImage.filterMode = FilterMode.Point;
        GetPointsFromTexture();
        pointsCount = CalculatePointNumber();
    }

    public int CalculatePointNumber()
    {
        int output = 0;
        foreach (var item in points)
        {
            if (item == 1) output++;
        }
        return output;
    }
}
//PRzygotowaæ funkcje tworzenia punktów z zdjêcia 