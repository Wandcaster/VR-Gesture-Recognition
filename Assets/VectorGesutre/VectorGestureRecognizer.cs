using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VectorGestureRecognizer : MonoBehaviour
{
    public List<RecognizeOutput> RecognizeGesture(VectorGesture gesture,List<IGesture> database)
    {
        List<RecognizeOutput> output = new List<RecognizeOutput>();
        foreach (var gestureFromDatabase in database)
        {
            float propability = CompareGesture(gesture, (VectorGesture)gestureFromDatabase);
            output.Add(new RecognizeOutput(gestureFromDatabase, propability));
        }
        output.Sort(delegate (RecognizeOutput x, RecognizeOutput y)
        {
            return x.probability.CompareTo(y.probability);
        });
        return output;
    }
    private float CompareGesture(VectorGesture gesture0, VectorGesture gesture1)
    {
        List<float> difference = new List<float>() ;
        for (int i = 0; i < gesture0.vectors.Length; i++)
        {
            difference.Add((gesture0.vectors[i] - gesture1.vectors[i]).magnitude);
        }
        return geometricMean(difference.ToArray(),difference.Count);
    }
    float geometricMean(float [] arr, int n)
    {
        // declare product variable and
        // initialize it to 1.
        float product = 1;

        // Compute the product of all the
        // elements in the array.
        for (int i = 0; i < n; i++)
            product = product * arr[i];

        // compute geometric mean through formula
        // pow(product, 1/n) and return the value
        // to main function.
        float gm = Mathf.Pow(product, (float)1 / n);
        return gm;
    }
}
