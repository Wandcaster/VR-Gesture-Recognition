using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureRecognizer : IGestureRecognizer
{
    public override List<RecognizeOutput> RecognizeGesture(Gesture gestureToRecognize,ref List<Gesture> gestureDatabase)
    {
        List<RecognizeOutput> output = new List<RecognizeOutput>();
        foreach (var gesture in gestureDatabase)
        {
            int identicalPoints = 0;
            int pointNumber = 0;
            RecognizeOutput tempOutput = new RecognizeOutput(gesture, 0);
            for (int i = 0; i < gesture.gestureImage.width; i++)
            {
                for (int j = 0; j < gesture.gestureImage.height; j++)
                {
                    if (gestureToRecognize.points[i, j] == 1)
                    {
                        pointNumber++;
                        if (gesture.points[i, j] == gestureToRecognize.points[i, j])
                        {
                            identicalPoints++;
                        }
                    }

                }
            }
            tempOutput.probability = identicalPoints / (float)pointNumber;
            output.Add(tempOutput);
        }
        output.Sort(delegate (RecognizeOutput x, RecognizeOutput y)
        {
            return y.probability.CompareTo(x.probability);
        });
        return output;
    }
}
