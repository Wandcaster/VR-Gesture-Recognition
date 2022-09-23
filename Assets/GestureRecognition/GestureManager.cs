using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class RecognizeOutput
{
    public Gesture recognizedGesture;
    public float probability;
    public RecognizeOutput(Gesture gesture, float probability)
    {
        recognizedGesture = gesture;
        this.probability = probability;
    }
}

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance { get; private set; }
    [Header("Configuration")]
    [SerializeField]
    private GesturePointsRecorder recorder;
    [SerializeField]
    private DrawGestureController drawController;
    [SerializeField]
    private GestureUIController gestureUIController;
    [SerializeField]
    private SteamVR_Action_Boolean isRecording;
    [SerializeField]
    private RawImage image;

    [Header("Data")]
    public bool AddGestureMode;
    public List<Gesture> gestureDatabase;
    public List<RecognizeOutput> RecognizeGesture(Gesture gestureToRecognize)
    {
        List<RecognizeOutput> output = new List<RecognizeOutput>();
        foreach (var gesture in gestureDatabase)
        {
            int identicalPoints = 0;
            int pointNumber = 0;
            RecognizeOutput tempOutput = new RecognizeOutput(gesture, 0);
            for (int i = 0; i < Gesture.imageSize; i++)
            {
                for (int j = 0; j < Gesture.imageSize; j++)
                {
                    if(gestureToRecognize.points[i, j] == 1)
                    {
                        pointNumber++;
                        if (gesture.points[i, j] == gestureToRecognize.points[i, j])
                        {
                            identicalPoints++;
                        }
                    }
                    
                }
            }
            tempOutput.probability = identicalPoints / (float)pointNumber;
            output.Add(tempOutput);
        }
        output.Sort(delegate (RecognizeOutput x, RecognizeOutput y)
        {
            return y.probability.CompareTo(x.probability);
        });
        return output;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Update()
    {
        if (isRecording.lastStateDown || Input.GetKeyDown("o"))
        {
            Debug.Log("StartRecording");
            recorder.StartCollectData();
        }
        if (isRecording.lastStateUp || Input.GetKeyDown("p"))
        {
            PointsData pointsData = recorder.StopCollectData();
            Texture2D gestureImage = drawController.DrawGesture(pointsData);

            Gesture gestureComponent = new Gesture("Gesture", gestureImage);
            if (AddGestureMode)
            {
                gestureDatabase.Add(gestureComponent);
                gestureUIController.AddGestureToUI(gestureImage, gestureComponent);
            }
            else
            {
                image.texture=RecognizeGesture(gestureComponent)[0].recognizedGesture.gestureImage;
            }
        }
    }
}
