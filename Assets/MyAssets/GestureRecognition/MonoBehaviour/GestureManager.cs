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
public class OnCreateGesture : UnityEvent<IGesture> { }

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance { get; private set; }
    [Header("Components configuration")]
    [SerializeField]
    private IDrawGestureController drawController;
    public IGestureRecognizer gestureRecognizer;
    public UIController gestureUIController; 
    public VectorGestureRecognizer vectorGestureRecognizer;
    [SerializeField]
    public TrailRenderer trailRenderer; 
    [Header("Key configuration")]
    [SerializeField]
    private SteamVR_Action_Boolean isRecording;
    [Header("Data")]
    string savePath = "Assets/Resources/SavedGestures"; //path to folder with gestureDatabases
    public IGestureDatabase gestureDatabase;
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
        foreach (var item in Resources.FindObjectsOfTypeAll<IGestureDatabase>())
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
            if (gestureUIController.type.value == 1) ImageMode(positions);
            else if(gestureUIController.type.value == 2)VectorMode(positions);

        }
    }
    private void VectorMode(Vector3[] positions)
    {
        StopTrialRenderer();
        VectorGesture gesture = new VectorGesture("Gesture", positions);
        OnCreateGesture.Invoke(gesture);

        //vectorGestureRecognizer.RecognizeGesture(gesture,gestureDatabase.gestures);
    }
    private void ImageMode(Vector3[] positions)
    {
        PointsData pointsData = new PointsData(new List<Vector2>(TransformPoints(positions)), 1000);
        StopTrialRenderer();
        Texture2D gestureImage = drawController.DrawGesture(pointsData);
        ImageGesture gestureComponent = new ImageGesture("Gesture", gestureImage, pointsData.rawPoints);
        OnCreateGesture.Invoke(gestureComponent);
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
            if(Math.Abs(points[i].x) > Math.Abs(points[i].z)) output[i].x = points[i].x;
            else output[i].x = points[i].z;

            output[i].y = points[i].y;
        }
        return output;
    }
    public List<GestureImageDatabase> LoadImageDatabase(string path)
    {
            List<GestureImageDatabase> gestureDatabases = new List<GestureImageDatabase>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            path = path.Remove(0, 17);
            foreach (var directory in dirInfo.GetDirectories())
            {
                GestureImageDatabase tempDatabase= Resources.Load<GestureImageDatabase>(path + "/" + directory.Name + "/" + directory.Name);
                if(tempDatabase!=null) gestureDatabases.Add(Resources.Load<GestureImageDatabase>(path + "/" + directory.Name + "/" + directory.Name));
            }
            return gestureDatabases;
    }
    public List<GestureVectorDatabase> LoadVectorDatabase(string path)
    {
        List<GestureVectorDatabase> gestureDatabases = new List<GestureVectorDatabase>();
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        path = path.Remove(0, 17);
        foreach (var directory in dirInfo.GetDirectories())
        {
            GestureVectorDatabase tempDatabase = Resources.Load<GestureVectorDatabase>(path + "/" + directory.Name + "/" + directory.Name);
            if(tempDatabase!=null)gestureDatabases.Add(tempDatabase);
        }
        return gestureDatabases;
    }
    
    public void AddGestureToDatabase(IGesture gesture)
    {
        gesture.Save(savePath + "/" + gestureDatabase.databaseName);
        gestureDatabase.gestures.Add(gesture);
        gestureDatabase.InitGestureDatabase();
        SetGestureID();
    }
    public void RemoveGestureFromDatabase(IGesture gesture)
    {
        gestureDatabase.gestures.Remove(gesture);
        Directory.Delete(savePath + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName,true);
        File.Delete(savePath + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName + ".meta");
        SetGestureID();
    }
    public void CreateDatabase(string name,int type)
    {
        if (name.Length == 0) return;
        if (type==1)//ImageType
        {
            Directory.CreateDirectory(savePath + "/" + name);
            GestureImageDatabase temp = ScriptableObject.CreateInstance<GestureImageDatabase>();
            temp.databaseName = name;
            AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
            gestureUIController.InitGestureDatabaseDropdown();
        }
        if(type==2)//VectorType
        {
            Directory.CreateDirectory(savePath + "/" + name);
            GestureVectorDatabase temp = ScriptableObject.CreateInstance<GestureVectorDatabase>();
            temp.databaseName = name;
            AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
            gestureUIController.InitGestureDatabaseDropdown();
        }
    }
}
