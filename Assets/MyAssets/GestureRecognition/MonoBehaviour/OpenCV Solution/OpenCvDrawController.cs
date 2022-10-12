using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCvDrawController : IDrawGestureController
{
    [SerializeField]
    int boldValue = 10;
    [SerializeField]
    int targetWidth = 200;
    [SerializeField]
    int targetHeight = 200;
    public override Texture2D DrawGesture(PointsData pointsData)
    {
        Texture2D output = new Texture2D(pointsData.expectedSize.x, pointsData.expectedSize.y, TextureFormat.RGBA32, false);
        for (int i = 0; i < pointsData.expectedSize.x; i++)
        {
            for (int j = 0; j < pointsData.expectedSize.y; j++)
            {
                if (pointsData.pointsAfterTransform[i, j] == 1) output.SetPixel(i, j, Color.black);
            }
        }
        output = BoldLines(output);
        output = ScaleTexture(output, targetWidth, targetHeight);
        return output;
    }
    public override Texture2D BoldLines(Texture2D texture)
    {
        List<Vector2Int> points = new List<Vector2Int>(); ;
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                if (texture.GetPixel(i, j).r == 0)
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
                    if ((item.x + i) < texture.width && (item.y + j) < texture.height && (item.x + i) > 0 && (item.y + j) > 0) texture.SetPixel(item.x + i, item.y + j, Color.black);
                    if ((item.x - i) < texture.width && (item.y + j) < texture.height && (item.x - i) > 0 && (item.y + j) > 0) texture.SetPixel(item.x - i, item.y + j, Color.black);
                    if ((item.x + i) < texture.width && (item.y - j) < texture.height && (item.x + i) > 0 && (item.y - j) > 0) texture.SetPixel(item.x + i, item.y - j, Color.black);
                    if ((item.x - i) < texture.width && (item.y - j) < texture.height && (item.x - i) > 0 && (item.y - j) > 0) texture.SetPixel(item.x - i, item.y - j, Color.black);
                }

            }
        }
        texture.Apply();
        return texture;
    }

    public override Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Mat image0 = OpenCvSharp.Unity.TextureToMat(source);
        Cv2.Resize(image0, image0, new Size(targetWidth, targetHeight));
        return OpenCvSharp.Unity.MatToTexture(image0);
    }
}
