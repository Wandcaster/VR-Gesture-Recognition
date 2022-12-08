using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VRGesureRecognition
{
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
        public Vector2[] vectors
        {
            get { return vectorGestureData.vectors; }
            set { vectorGestureData.vectors = value; }
        }

        private int pointsCount = 50;
        public VectorGesture(VectorGestureData data) : base(data)
        {
            vectorGestureData = data;
        }
        public VectorGesture(string name, Vector2[] positions) : base(null)
        {
            vectorGestureData = ScriptableObject.CreateInstance("VectorGestureData") as VectorGestureData;
            gestureName = name;
            positions = AddOrRemovePoints(positions);
            vectors = CreateVectors(positions);
        }
        private Vector2[] AddOrRemovePoints(Vector2[] points)
        {
            List<Vector2> output = new List<Vector2>(points);

            if (points.Length < pointsCount)
            {
                int step = Mathf.CeilToInt(points.Length / Mathf.Abs(points.Length - pointsCount))+1;
                Debug.Log(step);
                if (step <3) step = 3;
                int i = 0;
                while (output.Count != pointsCount)
                {
                    if (i+step+1 > output.Count)
                    {
                        step = Mathf.CeilToInt(output.Count / Mathf.Abs(output.Count - pointsCount)) + 1;
                        if (step <3) step = 3;
                        if (i + step + 1 > output.Count) i = 0;
                        else i = step;

                    }
                    output.Insert(i, (output[i] + output[i + 1]) / 2);
                    i += step;
                }
            }
            else
            {
                int step = Mathf.CeilToInt(points.Length / (points.Length - pointsCount))+1;
                Debug.Log(points.Length);
                int i = 0;
                while (output.Count != pointsCount)
                {
                    if (i > pointsCount)
                    {
                        step = Mathf.CeilToInt(output.Count / (output.Count - pointsCount)) + 1;
                        Debug.Log("New step:" + step);
                        if (step > pointsCount) i = 0;
                        else i = step;
                    }
                    Debug.Log(i);
                    output.RemoveAt(i);
                    i += step;
                }
            }
            return output.ToArray();
        }
        private Vector2[] CreateVectors(Vector2[] points)
        {
            List<Vector2> output = new List<Vector2>();
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
            //EditorUtility.FocusProjectWindow();
            Selection.activeObject = temp;
        }
    }
}
