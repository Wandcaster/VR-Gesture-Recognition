using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestureUIItem : MonoBehaviour
{
    public TMP_InputField gestureName;
    public RawImage gestureImage;
    public Button button;
    public Gesture gesture;
    private void Start()
    {
        button.onClick.AddListener(DestroyGestureUIItem);
    }
    public void DestroyGestureUIItem()
    {
        GestureManager.Instance.gestureDatabase.gestures.Remove(gesture);
        GestureManager.Instance.SetGestureID();
        Destroy(gameObject);
    }
    public void SetGestureName(String text)
    {
        gesture.gestureName = text;
    }
   
}
