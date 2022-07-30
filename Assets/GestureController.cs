using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using UnityEditor;

public class GestureController : MonoBehaviour
{
    [SerializeField]
    private Transform trackingPoint;
    private Transform spaceReferencePoint;
    [SerializeField]
    List<Vector2> points = new List<Vector2>();
    [SerializeField]
    Vector2 point;
    [SerializeField]
    int accuracy;
    [SerializeField]
    int scaleXTarget;
    [SerializeField]
    int scaleYTarget;
    [SerializeField]
    int boldValue;
    [SerializeField]
    float scaleValue;
    // Update is called once per frame

    public IEnumerator StartRecording()
    {
        GameObject emptyGO = new GameObject();
        spaceReferencePoint = emptyGO.transform;
        points.Clear();
        spaceReferencePoint.position = trackingPoint.position;
        points.Add(spaceReferencePoint.InverseTransformPoint(trackingPoint.position));
        while (true)
        {
            point = spaceReferencePoint.InverseTransformPoint(trackingPoint.position);
            if (point != points[points.Count - 1]) points.Add(point);
            yield return true;
        }
    }
    public void StopRecording()
    {
        StopAllCoroutines();

        float maxX = points.Max(x => x.x);
        float minX = points.Min(x => x.x);
        float maxY = points.Max(y => y.y);
        float minY = points.Min(y => y.y);

        float width = Mathf.Abs(maxX) + Mathf.Abs(minX);
        float height = Mathf.Abs(maxY) + Mathf.Abs(minY);
        

        Vector2 center = new Vector2(Mathf.Abs(width)*scaleValue/2, Mathf.Abs(height)*scaleValue/2); //œrodek punktów  œp

        int xImageSize = (int)(width * accuracy);
        int yImageSize = (int)(height * accuracy);

        xImageSize =(int)(xImageSize* scaleValue);
        yImageSize = (int)(yImageSize * scaleValue);

        if (xImageSize == 0) xImageSize = 1;
        if (yImageSize == 0) yImageSize = 1;

        int[,] tempPoints = new int[xImageSize, yImageSize];

        Texture2D output = new Texture2D(xImageSize, yImageSize, TextureFormat.RGBA32, false);

        float xCoordinate = 0; //Px'
        float yCoordinate = 0; //Py'

        float lastXCoordinate = 0;
        float lastYCoordinate = 0;

        bool firstIteration=true;
        foreach (var item in points)
        {
            bool skipPoint = false;

            xCoordinate = item.x;
            yCoordinate = item.y;

            //xCoordinate -= points[0].x;
            //yCoordinate -= points[0].y;

            xCoordinate += center.x;
            yCoordinate += center.y; 

            xCoordinate *= accuracy;
            yCoordinate *= accuracy;

            
            if (xCoordinate <= 0) skipPoint=true;
            if (yCoordinate <= 0) skipPoint = true;

            if (xCoordinate >= xImageSize-1) skipPoint = true;
            if (yCoordinate >= yImageSize-1) skipPoint = true;


            try
            {
                if(skipPoint==false)
                {
                    if (firstIteration == false) DrawLine(output, new Vector2((int)lastXCoordinate, (int)lastYCoordinate), new Vector2((int)xCoordinate, (int)yCoordinate), Color.black);
                    lastXCoordinate = xCoordinate;
                    lastYCoordinate = yCoordinate;
                    output.SetPixel((int)xCoordinate, (int)yCoordinate, Color.black);
                    tempPoints[(int)xCoordinate, (int)yCoordinate] = 1;
                    firstIteration = false;
                }
            }
            catch (Exception)
            {
                Debug.Log("Raw Point:" + item.x + " " + item.y);
                Debug.Log("Œrodek punktów" + center.x + " " + center.y);
                Debug.Log("Wielkoœæ obrazka:" + xImageSize + " " + yImageSize);
                Debug.Log("Punkt po obliczenaich:" + xCoordinate + " " + yCoordinate);


                throw;
            }
        }
        output = boldLines(output);
        output = ScaleTexture(output,scaleXTarget, scaleYTarget);
        output.Apply();

        //GestureManagerTests.tempImage.sprite = Sprite.Create(output, new Rect(0.0f, 0.0f,scaleXTarget, scaleYTarget), new Vector2(0.5f, 0.5f), 100);

        File.WriteAllBytes(Application.dataPath+ "\\Resources\\gesture.png", output.EncodeToPNG());
        Debug.Log(Application.dataPath + "\\Resources\\gesture.png");

        GameObject gesture = new GameObject("gesture");
        Gesture gestureComponent=gesture.AddComponent<Gesture>();
        gestureComponent.Init("Test", Resources.Load<Texture2D>("gesture"));
    }
    
    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Ceil(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    public void DrawLine(Texture2D t2D, Vector2 point1, Vector2 point2, Color col)
    {
        Vector2 t = point1;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)point2.x || (int)t.y != (int)point2.y)
        {
            t = Vector2.Lerp(point1, point2, ctr);
            ctr += frac;
            t2D.SetPixel((int)t.x, (int)t.y, col);
        }
    }

    public Texture2D boldLines(Texture2D texture)
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
                texture.SetPixel(item.x + i, item.y + i, Color.black);
                texture.SetPixel(item.x - i, item.y + i, Color.black);
                texture.SetPixel(item.x + i, item.y - i, Color.black);
                texture.SetPixel(item.x - i, item.y - i, Color.black);
            }
        }
        texture.Apply();
        return texture;
    }

}
