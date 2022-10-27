using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class GestureRecognizer : IGestureRecognizer
{
    List<RecognizeOutput> output = new List<RecognizeOutput>();
    Color[] pixelsToRecognize;
    public override List<RecognizeOutput> RecognizeGesture(Gesture gestureToRecognize, ref List<Gesture> gestureDatabase)
    {
        output.Clear();
        pixelsToRecognize = gestureToRecognize.gestureImage.GetPixels();
        foreach (var gesture in gestureDatabase)
        {
            int identicalPoints = 0;
            int i = 0;
            foreach (var color in gesture.gestureImage.GetPixels())
            {
                if (color == pixelsToRecognize[i]) identicalPoints++;
                i++;
            }
            output.Add(new RecognizeOutput(gesture, (float) identicalPoints / pixelsToRecognize.Count()));
        }

        output.Sort(delegate (RecognizeOutput x, RecognizeOutput y)
        {
            return y.probability.CompareTo(x.probability);
        });

        return output;
    }
}
