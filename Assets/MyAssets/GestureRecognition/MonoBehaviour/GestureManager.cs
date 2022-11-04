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
using Valve.VR.InteractionSystem;

[Serializable]
public class OnRecognition : UnityEvent<List<RecognizeOutput>> { }
public class OnCreateGesture : UnityEvent<Gesture> { }
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
    private IDrawGestureController drawController;
    [SerializeField]
    public IGestureRecognizer gestureRecognizer;
    public UIController gestureUIController;
    [SerializeField]
    public TrailRenderer trailRenderer; 
    [Header("Key configuration")]
    [SerializeField]
    private SteamVR_Action_Boolean isRecording;
    [Header("Data")]
    public GestureDatabase gestureDatabase;
    [Header("Events")]
    public OnRecognition OnRecognition;
    public OnCreateGesture OnCreateGesture=new OnCreateGesture();

    private Vector3[] positions;

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
        InitDatabases();
    }
    private static void InitDatabases()
    {
        Resources.LoadAll("SavedGestures\\");
        foreach (var item in Resources.FindObjectsOfTypeAll<GestureDatabase>())
        {
            item.InitGestureDatabase();
        }
    }
    public void Update()
    {
        if (isRecording.lastStateDown)
        {
            StartTrialRenderer();
        }
        if (isRecording.lastStateUp)
        {
            positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);
            PointsData pointsData = new PointsData(new List<Vector2>(TransformPoints(positions)), 1000);
            StopTrialRenderer();
            Texture2D gestureImage = drawController.DrawGesture(pointsData);
            Gesture gestureComponent = new Gesture("Gesture", gestureImage, pointsData.rawPoints);
            OnCreateGesture.Invoke(gestureComponent);
        }
    }
    private void StopTrialRenderer()
    {
        trailRenderer.time = 1;
        trailRenderer.Clear();
        trailRenderer.emitting = false;
    }
    private void StartTrialRenderer()
    {
        trailRenderer.Clear();
        trailRenderer.time = 1000;
        trailRenderer.emitting = true;
    }
    public void SetGestureID()
    {
        int i = 0;
        foreach (var item in gestureDatabase.gestures)
        {
            item.gestureID = i++;
        }
    }
    public Vector2[] TransformPoints(Vector3[] points)
    {
        Vector2[] output = new Vector2[points.Length];
        Vector3 startPos= points[0];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] -= startPos;
            points[i] = Vector3.ProjectOnPlane(points[i], Player.instance.bodyDirectionGuess);
            output[i].x = points[i].x;
            output[i].y = points[i].y;
        }
        return output;
    }
    public List<GestureDatabase> LoadDatabase(string path)
    {
        List<GestureDatabase> gestureDatabases = new List<GestureDatabase>();
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        path = path.Remove(0, 17);
        foreach (var directory in dirInfo.GetDirectories())
        {
            Debug.Log(path + "/" + directory.Name + "/" + directory.Name + ".asset");
            gestureDatabases.Add(Resources.Load<GestureDatabase>(path + "/" + directory.Name + "/" + directory.Name));
        }
        return gestureDatabases;
    }
    public void AddGestureToDatabase(Gesture gesture)
    {
        gesture.Save("Assets/Resources/SavedGestures" + "/" + gestureDatabase.databaseName);
        gestureDatabase.gestures.Add(gesture);
        gestureDatabase.InitGestureDatabase();
        SetGestureID();
    }
    public void RemoveGestureFromDatabase(Gesture gesture)
    {
        gestureDatabase.gestures.Remove(gesture);
        Directory.Delete("Assets/Resources/SavedGestures" + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName,true);
        File.Delete("Assets/Resources/SavedGestures" + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName + ".meta");
        SetGestureID();
    }
    public void CreateDatabase(string savePath, string name)
    {
        if (name.Length == 0) return;
        Directory.CreateDirectory(savePath + "/" + name);
        GestureDatabase temp = ScriptableObject.CreateInstance<GestureDatabase>();
        temp.databaseName = name;
        AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
        gestureUIController.InitGestureDatabaseDropdown();
    }
}
