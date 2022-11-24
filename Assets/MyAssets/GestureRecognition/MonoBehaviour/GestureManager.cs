using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public enum GestureType
{
    None,
    ImageGesture,
    VectorGesture
}


[Serializable]
public class OnRecognition : UnityEvent<List<RecognizeOutput>> { }
public class OnCreateGesture : UnityEvent<IGesture> { }

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance { get; private set; }
    [Header("Components configuration")]
    [SerializeField] private IDrawGestureController drawController;
    [SerializeField] private ImageGestureRecognizer imageGestureRecognizer;
    [SerializeField] private VectorGestureRecognizer vectorGestureRecognizer;
    [SerializeField] private TrailRenderer trackedPoint;
    public UIController gestureUIController;
    [Header("Key configuration")]
    [SerializeField]
    private SteamVR_Action_Boolean isRecording;
    [Header("Data")] private string savePath = "Assets/Resources/SavedGestures"; //path to folder with gestureDatabases
    public IGestureDatabase gestureDatabase;
    public GestureType gestureType;
    [Header("Events")]
    public OnRecognition OnRecognition;
    public OnCreateGesture OnCreateGesture=new OnCreateGesture();

    private Vector3[] positions;
    private float minPointDistance = 0.004F;

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
        InitTrialRenderer();
    }
    private void InitTrialRenderer()
    {
        trackedPoint.minVertexDistance = minPointDistance;
        trackedPoint.widthCurve = new AnimationCurve(new Keyframe[1] { new Keyframe(0, 0.01F) });
        if (trackedPoint.material == null)
        {
            Material material = new Material(Shader.Find("Transparent/Diffuse"));
            trackedPoint.material = material;
        }
    }
    private static void InitDatabases()
    {
        Resources.LoadAll("SavedGestures\\");
        foreach (var item in Resources.FindObjectsOfTypeAll<IGestureDatabase>())
        {
            item.InitGestureDatabase();
        }
    }
    private void Update()
    {
        if (isRecording.lastStateDown&&gestureDatabase!=null)
        {
            StartTrialRenderer();
        }
        if (isRecording.lastStateUp)
        {
            positions = new Vector3[trackedPoint.positionCount];
            trackedPoint.GetPositions(positions);
            if (gestureType == GestureType.ImageGesture) ImageMode(positions);
            else if(gestureType == GestureType.VectorGesture)VectorMode(positions);
        }
    }
    private void VectorMode(Vector3[] positions)
    {
        StopTrialRenderer();
        VectorGesture gesture = new VectorGesture("Gesture", positions);
        OnCreateGesture.Invoke(gesture);
        OnRecognition.Invoke(RecognizeGesture(gesture));
    }
    private void ImageMode(Vector3[] positions)
    {
        PointsData pointsData = new PointsData(new List<Vector2>(TransformPoints(positions)), 1000);
        StopTrialRenderer();
        Texture2D gestureImage = drawController.DrawGesture(pointsData);
        ImageGesture gestureComponent = new ImageGesture("Gesture", gestureImage, pointsData.rawPoints);
        OnCreateGesture.Invoke(gestureComponent);
        OnRecognition.Invoke(RecognizeGesture(gestureComponent));

    }
    private void StopTrialRenderer()
    {
        trackedPoint.time = 1;
        trackedPoint.Clear();
        trackedPoint.emitting = false;
    }
    private void StartTrialRenderer()
    {
        trackedPoint.Clear();
        trackedPoint.time = 1000;
        trackedPoint.emitting = true;
    }
    private void SetGestureID()
    {
        int i = 0;
        foreach (var item in gestureDatabase.gestures)
        {
            item.gestureID = i++;
        }
    }
    private Vector2[] TransformPoints(Vector3[] points)
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
    /// <summary>
    /// Load Image Gesture Databases form files.
    /// </summary>
    /// <returns>List of Image Gesture Databases</returns>
    public List<ImageGestureDatabase> LoadImageDatabase()
    {
            string path = savePath;
            List<ImageGestureDatabase> gestureDatabases = new List<ImageGestureDatabase>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            path = path.Remove(0, 17);
            foreach (var directory in dirInfo.GetDirectories())
            {
                ImageGestureDatabase tempDatabase= Resources.Load<ImageGestureDatabase>(path + "/" + directory.Name + "/" + directory.Name);
                if(tempDatabase!=null) gestureDatabases.Add(Resources.Load<ImageGestureDatabase>(path + "/" + directory.Name + "/" + directory.Name));
            }
            return gestureDatabases;
    }
    /// <summary>
    /// Load Vector Gesture Databases form files.
    /// </summary>
    /// <returns>List of Vector Gesture Databases</returns>
    public List<VectorGestureDatabase> LoadVectorDatabase()
    {
        string path = savePath;
        List<VectorGestureDatabase> gestureDatabases = new List<VectorGestureDatabase>();
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        path = path.Remove(0, 17);
        foreach (var directory in dirInfo.GetDirectories())
        {
            VectorGestureDatabase tempDatabase = Resources.Load<VectorGestureDatabase>(path + "/" + directory.Name + "/" + directory.Name);
            if(tempDatabase!=null)gestureDatabases.Add(tempDatabase);
        }
        return gestureDatabases;
    }
    /// <summary>
    /// Add gesture to current loaded database
    /// </summary>
    /// <param name="gesture">Gesture to add to the database</param>
    public void AddGestureToDatabase(IGesture gesture)
    {
        gesture.Save(savePath + "/" + gestureDatabase.databaseName);
        gestureDatabase.gestures.Add(gesture);
        gestureDatabase.InitGestureDatabase();
        SetGestureID();
    }
    /// <summary>
    /// Remove gesture from current loaded database
    /// </summary>
    /// <param name="gesture">Gesture to remove from the database</param>
    public void RemoveGestureFromDatabase(IGesture gesture)
    {
        gestureDatabase.gestures.Remove(gesture);
        Directory.Delete(savePath + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName,true);
        File.Delete(savePath + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName + ".meta");
        SetGestureID();
    }
    /// <summary>
    /// Create new gesture database and save it as file.
    /// </summary>
    /// <param name="name">Name of new gesture database</param>
    /// <param name="type">Type of gesture that database stores</param>
    public void CreateDatabase(string name,GestureType type)
    {
        if (name.Length == 0) return;
        if (type==GestureType.ImageGesture)//ImageType
        {
            Directory.CreateDirectory(savePath + "/" + name);
            ImageGestureDatabase temp = ScriptableObject.CreateInstance<ImageGestureDatabase>();
            temp.databaseName = name;
            AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
            gestureUIController.InitGestureDatabaseDropdown();
        }
        if(type == GestureType.VectorGesture)//VectorType
        {
            Directory.CreateDirectory(savePath + "/" + name);
            VectorGestureDatabase temp = ScriptableObject.CreateInstance<VectorGestureDatabase>();
            temp.databaseName = name;
            AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
            gestureUIController.InitGestureDatabaseDropdown();
        }
    }
    /// <summary>
    /// Compare all gesture stored in database
    /// </summary>
    /// <param name="gesture">Gesture that will be compared to gestures form database</param>
    /// <returns>List of RecognizeOutput that contains gesture from database and probability returned from gesture recognition</returns>
    public List<RecognizeOutput> RecognizeGesture(IGesture gesture)
    {
        VectorGesture vectorGesture = gesture as VectorGesture;
        List<RecognizeOutput> output;
        if (vectorGesture != null)
        {
            output= vectorGestureRecognizer.RecognizeGesture(vectorGesture, gestureDatabase.gestures);
        }
        else
        {
            output= imageGestureRecognizer.RecognizeGesture((ImageGesture)gesture,gestureDatabase.gestures);
        }
        return output;
    }
}
