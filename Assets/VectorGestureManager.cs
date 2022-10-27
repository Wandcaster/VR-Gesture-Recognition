using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VectorGestureManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    TrailRenderer trailRenderer;
    [Header("Key configuration")]
    [SerializeField]
    private SteamVR_Action_Boolean isRecording;
    [Header("Settings")]
    [SerializeField]
    private int pointsCount;
    [SerializeField]
    private bool recognitionMode;
    [SerializeField]
    string gestureName;
    [Header("Data")]
    [SerializeField]
    Vector3[] positions;
    [SerializeField]
    List<Vector3> vectors;
    [SerializeField]
    List<VectorGesture> database = new List<VectorGesture>();

    [Serializable]
    struct VectorGesture
    {
        public string name;
        public List<Vector3> vectors;
        public VectorGesture(string name, List<Vector3> vectors)
        {
            this.name = name;
            this.vectors = vectors;
        }
    }
    private void Update()
    {
        if (isRecording.lastStateDown)
        {
            trailRenderer.time = 1000;
            trailRenderer.emitting = true;
        }
        if (isRecording.lastStateUp)
        {
            trailRenderer.time = 1;
            trailRenderer.emitting = false;

            positions = new Vector3[trailRenderer.positionCount];
            positions= TransformPoints(positions);

            trailRenderer.GetPositions(positions);
            positions = AddOrRemovePoints(positions);
            vectors = CreateVectors(positions);
            VectorGesture gesture = new VectorGesture(gestureName,vectors);
            if (recognitionMode)
            {
                RecognizeGesture(gesture);
            }
            else
            {
                database.Add(gesture);
            }
        }
    }
    private Vector3[] AddOrRemovePoints(Vector3[] points)
    {
        //if (points.Length < pointsCount / 2) throw EBlockQueueError;
        List<Vector3> output = new List<Vector3>(points);
        int missingVectors = pointsCount - points.Length;
        
        if (points.Length<pointsCount)
        {
            int step = pointsCount / missingVectors;
            //Debug.Log(step);
            int i = 0;
            while (output.Count<pointsCount)
            {
                if (output.Count <= i) i = 0;
                output.Insert(i+1, (output[i] + output[i+1])/2);
                i += step;
            }
        }
        else
        {
            int step = Mathf.Abs(missingVectors/ pointsCount );
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
    private List<Vector3> CreateVectors(Vector3[] points)
    {
        List<Vector3> output = new List<Vector3>();
        for (int i = 0; i < points.Length-1; i++)
        {
            output.Add((points[i + 1] - points[i]).normalized); //liczyæ wektor z wiêkszej ilosæi punktów
        }
        return output;
    }
    private void RecognizeGesture(VectorGesture gesture)
    {
        foreach (var gestureFromDatabase in database)
        {
            Debug.Log(CompareGesture(gesture, gestureFromDatabase)+" "+ gestureFromDatabase.name);
        }
    }
    private float CompareGesture(VectorGesture gesture0, VectorGesture gesture1)
    {
        float difference = 0;
        for (int i = 0; i < gesture0.vectors.Count; i++)
        {
            difference += (gesture0.vectors[i] - gesture1.vectors[i]).magnitude;
        }
        return difference;
    }
    private Vector3[] TransformPoints(Vector3[] positions)
    {
        List<Vector3> output = new List<Vector3>();
        foreach (var item in positions)
        {
            output.Add(Vector3.ProjectOnPlane(item, Player.instance.bodyDirectionGuess));
        }
        return output.ToArray();
    }
}
