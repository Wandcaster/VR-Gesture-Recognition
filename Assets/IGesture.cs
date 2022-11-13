using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IGesture
{
    public int gestureID
    {
        get { return gestureData.ID; }
        set { gestureData.ID = value; }
    }
    public string gestureName
    {
        get { return gestureData.gestureName; }
        set { gestureData.gestureName = value; }
    }
    public IGestureData gestureData= new IGestureData();
    public IGesture(IGestureData gestureData)
    {
        this.gestureData = gestureData;
    }
    public virtual void Save(string path) { }
}
