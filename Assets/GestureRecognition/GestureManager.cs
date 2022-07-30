using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RecognizeOutput
{
    public Gesture recognizedGesture;
    public float probability;
    public RecognizeOutput(Gesture gesture, float probability)
    {
        recognizedGesture = gesture;
        this.probability = probability;
    }
}

public class GestureManager : MonoBehaviour
{
    [SerializeField]
    List<Gesture> gestureDatabase;
    
    public List<RecognizeOutput> RecognizeGesture(Gesture gestureToRecognize)
    {
        List<RecognizeOutput> output = new List<RecognizeOutput>();
        foreach (var gesture in gestureDatabase)
        {
            int identicalPoints = 0;
            RecognizeOutput tempOutput = new RecognizeOutput(gesture, 0);
            for (int i = 0; i < Gesture.imageSize; i++)
            {
                for (int j = 0; j < Gesture.imageSize; j++)
                {
                    if (gesture.points[i, j] == gestureToRecognize.points[i, j])
                    {
                        identicalPoints++;
                    }
                }
            }
            tempOutput.probability = identicalPoints / math.pow(Gesture.imageSize,2);
            output.Add(tempOutput);
        }
        output.Sort(delegate (RecognizeOutput x, RecognizeOutput y)
        {
            return y.probability.CompareTo(x.probability);
        });
        return output;
    }
}
