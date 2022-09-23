using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GesturePointsRecorder))]
[RequireComponent(typeof(DrawGestureController))]
public class GestureController : MonoBehaviour
{
    private GesturePointsRecorder recorder;
    private DrawGestureController drawController;
    public PointsData lastPointData;
    public Texture2D lastGestureImage;

    public void StartRecording()
    {
       // recorder.StartCoroutine(recorder.StartCollectData());
    }
    public void StopRecording()
    {
        lastPointData=recorder.StopCollectData();
    }
    public void DrawGesture()
    {
        lastGestureImage=drawController.DrawGesture(lastPointData);
    }
    public void SaveGesture()
    {
        drawController.SaveImageToFile(lastGestureImage, "");
    }
    private void Start()
    {
        recorder = GetComponent<GesturePointsRecorder>();
        drawController = GetComponent<DrawGestureController>();
    }
}
