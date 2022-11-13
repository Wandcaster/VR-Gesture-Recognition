using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCvGestureRecognizer : IGestureRecognizer
{
    public override List<RecognizeOutput> RecognizeGesture(Gesture gestureToRecognize, List<IGesture> gestureDatabase)
    {
        List<RecognizeOutput> output = new List<RecognizeOutput>();
        foreach (Gesture gesture in gestureDatabase)
        {
            double propability= CompareImages(gestureToRecognize.gestureImage, gesture.gestureImage);
            output.Add(new RecognizeOutput(gesture, (float)propability));
        }
        output.Sort(delegate (RecognizeOutput x, RecognizeOutput y)
        {
            return y.probability.CompareTo(x.probability);
        });
        return output;
    }
    private double CompareImages(Texture2D texture0, Texture2D texture1)
    {
        Mat image0 = OpenCvSharp.Unity.TextureToMat(texture0);
        Mat image1 = OpenCvSharp.Unity.TextureToMat(texture1);

        int width = texture0.width;
        int height = texture0.height;

        double errorL2 = Cv2.Norm(image0, image1);
        return 1 - errorL2 / (height * width);
    }
}
