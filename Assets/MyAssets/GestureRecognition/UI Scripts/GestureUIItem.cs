using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GestureUIItem : MonoBehaviour
{
    public RawImage gestureImage;
    public Button button;
    public Gesture gesture;
    private void Start()
    {
        button.onClick.AddListener(DestroyGestureUIItem);
    }
    public void DestroyGestureUIItem()
    {
        GestureManager.Instance.gestureDatabase.Remove(gesture);
        GestureManager.Instance.SetGestureID();
        Destroy(gameObject);
    }
    public void SetGestureName(String text)
    {
        gesture.gestureName = text;
    }
    public void SaveGesture(string path)
    {
        gesture.Save(path + "/" + gesture.gestureName+"/"+gesture.gestureName);
    }
}
