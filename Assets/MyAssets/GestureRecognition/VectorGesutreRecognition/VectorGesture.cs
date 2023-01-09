using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
            if (output.Count < 10) return null;
            if (points.Length < pointsCount)
            {
                float step = Mathf.Abs((pointsCount / (float)(output.Count - pointsCount)) -1);
               
                float i = 0;
                while (output.Count != pointsCount)
                {
                    i += step;
                    if (i < output.Count-1)
                    {
                        output.Insert(Mathf.FloorToInt(i), (output[Mathf.FloorToInt(i)] + output[Mathf.FloorToInt(i) + 1]) / 2);
                    }
                    else
                    {
                        i = 0;
                        step = Mathf.Abs((pointsCount / (float)(output.Count - pointsCount)) - 1);
                        if (step == pointsCount + 1) step = pointsCount / 2;
                    }
                }
            }
            else
            {
                float step = (pointsCount / (float)(output.Count - pointsCount) )+1;
                float i=0;
                while (output.Count != pointsCount)
                {
                    i += step;
                    if (i < output.Count - 2)
                    {
                        output.RemoveAt(Mathf.FloorToInt(i));
                    }
                    else
                    {
                        i = 0;
                        step = (pointsCount / (float)(output.Count - pointsCount)) + 1;
                        if (step == pointsCount + 1) step = pointsCount / 2;
                    }
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
#if UNITY_EDITOR
            path += "/" + gestureName;
            Directory.CreateDirectory(path);
            VectorGestureData temp = ScriptableObject.CreateInstance<VectorGestureData>();
            EditorUtility.CopySerialized(vectorGestureData, temp);
            AssetDatabase.CreateAsset(temp, path + "/" + gestureName + ".asset");
            //EditorUtility.FocusProjectWindow();
            Selection.activeObject = temp;
#endif
        }
    }
}
