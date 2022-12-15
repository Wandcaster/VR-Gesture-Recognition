using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGesureRecognition;

public class DebugResult : MonoBehaviour
{

    public void OnRecognition(List<RecognizeOutput> outputs)
    {
        foreach (var item in outputs)
        {
            Debug.Log(item.recognizedGesture.gestureName + ": " + item.probability);
        }
    }
}
