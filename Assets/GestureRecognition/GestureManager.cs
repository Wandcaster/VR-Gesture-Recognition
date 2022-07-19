using System.Collections;
using System.Collections.Generic;
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
    
    public RecognizeOutput RecognizeGesture(Gesture gestureToRecognize)
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
                    if (gesture.points[i, j] == 1 && gestureToRecognize.points[i, j] == 1)
                    {
                        identicalPoints++;
                    }
                }
            }
            tempOutput.probability = identicalPoints / gestureToRecognize.pointsCount;
            output.Add(tempOutput);
        }
        float greatestProbability = 0;
        RecognizeOutput recognizeOutput=null;
        foreach (var item in output)
        {
            if (item.probability > greatestProbability)
            {
                greatestProbability = item.probability;
                recognizeOutput = item;
            }
        }
        return recognizeOutput;
    }
}
