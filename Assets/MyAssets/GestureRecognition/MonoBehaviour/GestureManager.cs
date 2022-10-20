using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
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
    [SerializeField]
    public TrailRenderer trailRenderer; 
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
    public GestureDatabase gestureDatabase;
    public string databaseName;
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
    private void Start()
    {
        foreach (var item in Resources.FindObjectsOfTypeAll<GestureDatabase>())
        {
            item.InitGestureDatabase();
        } 
    }
    public void Update()
    {
        if (isRecording.lastStateDown || Input.GetKeyDown("o"))
        {
            gestureRecorder.StartCollectData();
            trailRenderer.time = 1000;
            trailRenderer.emitting = true;
        }
        if (isRecording.lastStateUp || Input.GetKeyDown("p"))
        {
            trailRenderer.time = 1;
            trailRenderer.emitting = false;
            PointsData pointsData = gestureRecorder.StopCollectData();
            Texture2D gestureImage = drawController.DrawGesture(pointsData);
            Gesture gestureComponent = new Gesture("Gesture", gestureImage, pointsData.rawPoints);
            if (AddGestureMode)
            {
                if (gestureDatabase == null) gestureDatabase = ScriptableObject.CreateInstance<GestureDatabase>();

                gestureDatabase.gestures.Add(gestureComponent);
                SetGestureID();
                gestureUIController.AddGestureToUI(gestureImage, gestureComponent);
            }
            else
            {
                List<RecognizeOutput> output = gestureRecognizer.RecognizeGesture(gestureComponent, ref gestureDatabase.gestures);
                SetUI(output, gestureComponent);
                OnRecognition.Invoke(output);
            }
        }
    }

    private void SetUI(List<RecognizeOutput> recognizeOutput, Gesture gestureComponent)
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
        foreach (var item in gestureDatabase.gestures)
        {
            item.gestureID = i++;
        }
    }

    public void SaveDatabase(string path)
    {
        Directory.CreateDirectory(path + "/" + databaseName);

        GestureDatabase temp = ScriptableObject.CreateInstance<GestureDatabase>();
        temp.databaseName = databaseName;
        AssetDatabase.CreateAsset(temp, path + "/" + databaseName + "/" + databaseName + ".asset");

        foreach (var item in gestureDatabase.gestures)
        {
            item.Save(path + "/" + databaseName,ref temp);
        }

        
    }
    public void LoadDatabase(GestureDatabase gestureDatabase)
    {
        this.gestureDatabase = gestureDatabase;
        gestureUIController.ClearUI();
        foreach (var gesture in this.gestureDatabase.gestures)
        {
            gestureUIController.AddGestureToUI(gesture.gestureImage, gesture);
        }
        gestureUIController.gestureName.text = gestureDatabase.databaseName;
    }
    public void SetGestureDatabaseName(string name)
    {
        if (gestureDatabase != null)
        {
            databaseName = name;
        }

    }
}
