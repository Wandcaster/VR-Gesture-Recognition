using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
namespace VRGesureRecognition
{

    [Serializable]
    public class OnRecognition : UnityEvent<List<RecognizeOutput>> { }
    public class OnCreateGesture : UnityEvent<IGesture> { }
    public class GestureManager : Singleton<GestureManager>
    {
        [Header("Components configuration")]
        [Tooltip("Draw gesture image from points")]
        [SerializeField] private IDrawGestureController imageDrawController;
        [Tooltip("Recognize image gestures")]
        [SerializeField] private ImageGestureRecognizer imageGestureRecognizer;
        [Tooltip("Recognize vector gestures")]
        [SerializeField] private VectorGestureRecognizer vectorGestureRecognizer;
        [Tooltip("Object that position is traced, for example finger tip or wand tip")]
        public TrailRenderer trackedPoint;
        private UIController gestureUIController;
        [Header("Key configuration")]
        [Tooltip("Button for trigger start collecting point on pressDown and start recognizing on pressUp")]
        public SteamVR_Action_Boolean isRecording;
        [Header("Data")]
        private string savePath = "Assets/Resources/SavedGestures"; //path to folder with gestureDatabases
        [Tooltip("Database that contain gestures")]
        public IGestureDatabase gestureDatabase;
        [Header("Events")]
        [Tooltip("Event trigger when recognition happen")]
        public OnRecognition OnRecognition;
        [Tooltip("Event trigger when gesture is created")]
        public OnCreateGesture OnCreateGesture = new OnCreateGesture();
        [Tooltip("If is false than recognition is stopped")]
        public bool isEnabled;

        private Vector3[] positions;
        [HideInInspector]
        public float pointDistanceOnImage = 0.004F;
        [HideInInspector]
        public float pointDistanceOnVector = 0.01F;

        private void Start()
        {
            InitDatabases();
            InitTrialRenderer();
            gestureUIController = FindObjectOfType<UIController>();
        }
        private void InitTrialRenderer()
        {
            trackedPoint.minVertexDistance = pointDistanceOnImage;
            trackedPoint.widthCurve = new AnimationCurve(new Keyframe[1] { new Keyframe(0, 0.01F) });
            if (trackedPoint.material == null)
            {
                Material material = new Material(Shader.Find("Transparent/Diffuse"));
                trackedPoint.material = material;
            }
            StopTrialRenderer();
        }
        private void InitDatabases()
        {
            Resources.LoadAll("SavedGestures\\");
            foreach (var item in Resources.FindObjectsOfTypeAll<IGestureDatabase>())
            {
                item.InitGestureDatabase();
            }
        }
        private void Update()
        {
            if (!isEnabled) return;
            if (isRecording.lastStateDown && gestureDatabase != null)
            {
                StartTrialRenderer();
            }
            if (isRecording.lastStateUp)
            {
                Recognize();
            }
        }

        public void Recognize()
        {
            positions = new Vector3[trackedPoint.positionCount];
            trackedPoint.GetPositions(positions);

            if (gestureDatabase as ImageGestureDatabase != null) ImageMode(positions);
            else VectorMode(positions);
        }

        private void VectorMode(Vector3[] positions)
        {
            StopTrialRenderer();
            VectorGesture gesture = new VectorGesture("Gesture", TransformPoints(positions));
            OnCreateGesture.Invoke(gesture);
            if(gestureDatabase.gestures.Count>1) OnRecognition.Invoke(RecognizeGesture(gesture));
        }
        private void ImageMode(Vector3[] positions)
        {
            PointsData pointsData = new PointsData(new List<Vector2>(TransformPoints(positions)), 1000);
            StopTrialRenderer();
            Texture2D gestureImage = imageDrawController.DrawGesture(pointsData);
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
        public void StartTrialRenderer()
        {
            trackedPoint.Clear();
            trackedPoint.time = 1000;
            trackedPoint.emitting = true;
            if (gestureDatabase as ImageGestureDatabase != null) trackedPoint.minVertexDistance = pointDistanceOnImage;
            else trackedPoint.minVertexDistance = pointDistanceOnVector;
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
            Vector3 startPos = points[0];
            Vector3 bodyDirection = trackedPoint.transform.forward;
            bodyDirection.y = 0;
            
            for (int i = 0; i < points.Length; i++)
            {
                points[i] -= startPos;
                points[i] = Vector3.ProjectOnPlane(points[i], bodyDirection);
                if (Math.Abs(points[i].x) > Math.Abs(points[i].z)) output[i].x = points[i].x;
                else output[i].x = points[i].z;

                output[i].y = points[i].y;
            }
            if (bodyDirection.x > bodyDirection.z)
            {
                for (int i = 0; i < output.Length; i++)
                {
                    output[i].x = -output[i].x;
                }
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
                ImageGestureDatabase tempDatabase = Resources.Load<ImageGestureDatabase>(path + "/" + directory.Name + "/" + directory.Name);
                if (tempDatabase != null) gestureDatabases.Add(Resources.Load<ImageGestureDatabase>(path + "/" + directory.Name + "/" + directory.Name));
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
                if (tempDatabase != null) gestureDatabases.Add(tempDatabase);
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
            if (gestureDatabase.gestures == null) gestureDatabase.gestures = new List<IGesture>();
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
            Directory.Delete(savePath + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName, true);
            File.Delete(savePath + "/" + gestureDatabase.databaseName + "/" + gesture.gestureName + ".meta");
            SetGestureID();
        }
        /// <summary>
        /// Create new gesture database and save it as file.
        /// </summary>
        /// <param name="name">Name of new gesture database</param>
        /// <param name="type">Type of gesture that database stores</param>
        public void CreateDatabase(string name, GestureType type)
        {
#if UNITY_EDITOR
            if (name.Length == 0) return;
            if (type == GestureType.ImageGesture)//ImageType
            {
                Directory.CreateDirectory(savePath + "/" + name);
                ImageGestureDatabase temp = ScriptableObject.CreateInstance<ImageGestureDatabase>();
                temp.databaseName = name;
                AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
                gestureUIController.InitGestureDatabaseDropdown();
            }
            if (type == GestureType.VectorGesture)//VectorType
            {
                Directory.CreateDirectory(savePath + "/" + name);
                VectorGestureDatabase temp = ScriptableObject.CreateInstance<VectorGestureDatabase>();
                temp.databaseName = name;
                AssetDatabase.CreateAsset(temp, savePath + "/" + name + "/" + name + ".asset");
                gestureUIController.InitGestureDatabaseDropdown();
            }
#endif
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
                output = vectorGestureRecognizer.RecognizeGesture(vectorGesture, gestureDatabase.gestures);
            }
            else
            {
                output = imageGestureRecognizer.RecognizeGesture((ImageGesture)gesture, gestureDatabase.gestures);
            }
            return output;
        }
    }
}
