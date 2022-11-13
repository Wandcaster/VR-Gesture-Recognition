using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class VectorGesture : IGesture
{
    VectorGestureData vectorGestureData
    {
        get
        {
            return (VectorGestureData)gestureData;
        }
        set
        {
            gestureData = value;
        }
    }
    public Vector3[] vectors
    {
        get { return vectorGestureData.vectors; }
        set { vectorGestureData.vectors= value; }
    }

    private int pointsCount=50;
    public VectorGesture(string name, Vector3[] positions):base(null)
    {
        vectorGestureData = ScriptableObject.CreateInstance("VectorGestureData") as VectorGestureData;
        gestureName = name;
        positions = AddOrRemovePoints(positions);
        vectors = CreateVectors(positions);
    }
    private Vector3[] AddOrRemovePoints(Vector3[] points)
    {
        //if (points.Length < pointsCount / 2) throw EBlockQueueError;
        List<Vector3> output = new List<Vector3>(points);
        int missingVectors = pointsCount - points.Length;

        if (points.Length < pointsCount)
        {
            int step = pointsCount / missingVectors;
            //Debug.Log(step);
            int i = 0;
            while (output.Count < pointsCount)
            {
                if (output.Count <= i) i = 0;
                output.Insert(i + 1, (output[i] + output[i + 1]) / 2);
                i += step;
            }
        }
        else
        {
            int step = Mathf.Abs(missingVectors / pointsCount);
            int i = 0;
            while (output.Count > pointsCount)
            {
                if (output.Count <= i) i = 0;
                output.RemoveAt(i);
                i += step;
            }
        }
        return output.ToArray();
    }
    private Vector3[] CreateVectors(Vector3[] points)
    {
        List<Vector3> output = new List<Vector3>();
        for (int i = 0; i < points.Length - 1; i++)
        {
            output.Add((points[i + 1] - points[i]).normalized); //liczyæ wektor z wiêkszej ilosæi punktów
        }
        return output.ToArray();
    }
    public override void Save(string path)
    {
        path += "/" + gestureName;
        Directory.CreateDirectory(path);
        VectorGestureData temp = ScriptableObject.CreateInstance<VectorGestureData>();
        EditorUtility.CopySerialized(vectorGestureData, temp);
        AssetDatabase.CreateAsset(temp, path + "/" + gestureName + ".asset");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = temp;
    }
}
