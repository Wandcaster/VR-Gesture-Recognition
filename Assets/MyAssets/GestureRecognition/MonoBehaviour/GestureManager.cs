using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Valve.VR;

[Serializable]
public class OnRecognition : UnityEvent<List<RecognizeOutput>> { }
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
    [Header("Components configuration")]
    [SerializeField]
    private IGesturePointsRecorder gestureRecorder;
    [SerializeField]
    private IDrawGestureController drawController;
    [SerializeField]
    private IGestureRecognizer gestureRecognizer;
    [SerializeField]
    private GestureUIController gestureUIController;
    [Header("Key configuration")]
    [SerializeField]
    private SteamVR_Action_Boolean isRecording;
    [Header("UI configuration")]
    [SerializeField]
    private RawImage recognizedImage;
    [SerializeField]
    private RawImage createdImage;
    [SerializeField]
    TextMeshProUGUI PropabilityText;

    [Header("Data")]    
    public bool AddGestureMode;
    public List<Gesture> gestureDatabase;
    [Header("Events")]
    public OnRecognition OnRecognition;
    
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
            gestureRecorder.StartCollectData();
        }
        if (isRecording.lastStateUp || Input.GetKeyDown("p"))
        {
            PointsData pointsData = gestureRecorder.StopCollectData();
            Texture2D gestureImage = drawController.DrawGesture(pointsData);
            Gesture gestureComponent = new Gesture("Gesture", gestureImage, pointsData.rawPoints);
            if (AddGestureMode)
            {
                gestureDatabase.Add(gestureComponent);
                SetGestureID();
                gestureUIController.AddGestureToUI(gestureImage, gestureComponent);
            }
            else
            {
                List<RecognizeOutput> output = gestureRecognizer.RecognizeGesture(gestureComponent,ref gestureDatabase);
                SetUI(output, gestureComponent);
                OnRecognition.Invoke(output);
            }
        }
    }

    private void SetUI(List<RecognizeOutput> recognizeOutput,Gesture gestureComponent)
    {
        RecognizeOutput result = recognizeOutput[0];
        recognizedImage.texture = result.recognizedGesture.gestureImage;
        createdImage.texture = gestureComponent.gestureImage;
        PropabilityText.text = "Propability: " + result.probability;
        Debug.Log(result.probability);
    }

    public void SetGestureID()
    {
        int i = 0;
        foreach (var item in gestureDatabase)
        {
            item.gestureID = i++;
        }
    }
}
