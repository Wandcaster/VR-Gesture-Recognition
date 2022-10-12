using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGesturePointsRecorder:MonoBehaviour
{
    public abstract void StartCollectData();
    public abstract PointsData StopCollectData();
}
