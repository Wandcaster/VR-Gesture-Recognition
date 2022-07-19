using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManagerTests : MonoBehaviour
{
    [SerializeField]
    GestureManager gestureManager;
    [SerializeField]
    Gesture gestureToRecognize;
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown("x"))
        {

            RecognizeOutput gesture = gestureManager.RecognizeGesture(gestureToRecognize);
            Debug.Log(gesture.recognizedGesture.gestureName + " Probability:" + gesture.probability);
        }
    }
}
