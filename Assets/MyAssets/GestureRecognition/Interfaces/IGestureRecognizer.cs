using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGestureRecognizer:MonoBehaviour
{
    public abstract List<RecognizeOutput> RecognizeGesture(Gesture gestureToRecognize, List<Gesture> gestureDatabase);
}
