using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using UnityEditor;

public class DrawGestureController : IDrawGestureController
{
    [SerializeField]
    int scaleXTarget=200;
    [SerializeField]
    int scaleYTarget=200;
    [SerializeField]
    int boldValue=10;
    
    /// <summary>
    /// Create texture from pointData
    /// </summary>
    /// <param name="pointsData">Gesture points</param>
    /// <returns>Gesture image</returns>
    public override Texture2D DrawGesture(PointsData pointsData)
    {
        Texture2D output = new Texture2D(pointsData.expectedSize.x, pointsData.expectedSize.y, TextureFormat.RGBA32, false);

        for (int i = 0; i < pointsData.expectedSize.x; i++)
        {
            for (int j = 0; j < pointsData.expectedSize.y; j++)
            {
                if(pointsData.pointsAfterTransform[i,j]==1) output.SetPixel(i, j, Color.black);
            }
        }
        output = BoldLines(output);
        output = ScaleTexture(output,scaleXTarget, scaleYTarget);
        output.Apply();
        return output;
    }

    protected override Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color32[] rpixels = result.GetPixels32(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Ceil(px / targetWidth)));
        }
        result.SetPixels32(rpixels, 0);
        result.Apply();
        return result;
    }

    protected override Texture2D BoldLines(Texture2D texture)
    {
        List<Vector2Int> points = new List<Vector2Int>(); ;
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                if(texture.GetPixel(i,j).r==0)
                {
                    points.Add(new Vector2Int(i, j));
                }
            }            
        }
        foreach (var item in points)
        {
            for (int i = 1; i < boldValue; i++)
            {
                for (int j = 0; j < boldValue; j++)
                {
                    if((item.x + i)<texture.width&&(item.y + j)<texture.height && (item.x + i) > 0 && (item.y + j) > 0) texture.SetPixel(item.x + i, item.y + j, Color.black);
                    if ((item.x - i) < texture.width && (item.y + j) < texture.height && (item.x - i) >0 && (item.y + j) >0) texture.SetPixel(item.x - i, item.y + j, Color.black);
                    if ((item.x + i) < texture.width && (item.y - j) < texture.height && (item.x + i) >0 && (item.y - j) >0) texture.SetPixel(item.x + i, item.y - j, Color.black);
                    if ((item.x - i) < texture.width && (item.y - j) < texture.height && (item.x - i) >0 && (item.y - j) >0) texture.SetPixel(item.x - i, item.y - j, Color.black);
                }
                
            }
        }
        texture.Apply();
        return texture;
    }

    //public void SaveImageToFile(Texture2D image, string path)
    //{
    //    File.WriteAllBytes(Application.dataPath + "\\Resources\\gesture.png", image.EncodeToPNG());
    //    Debug.Log(Application.dataPath + "\\Resources\\gesture.png");
    //}

    //public void DrawLine(Texture2D t2D, Vector2 point1, Vector2 point2, Color col)
    //{
    //    Vector2 t = point1;
    //    float frac = 1 / Mathf.Sqrt(Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2));
    //    float ctr = 0;

    //    while ((int)t.x != (int)point2.x || (int)t.y != (int)point2.y)
    //    {
    //        t = Vector2.Lerp(point1, point2, ctr);
    //        ctr += frac;
    //        t2D.SetPixel((int)t.x, (int)t.y, col);
    //    }
    //}

}
