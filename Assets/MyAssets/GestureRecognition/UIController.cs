using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace VRGesureRecognition
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        GameObject createModePanel;
        [SerializeField]
        GameObject inspectModePanel;
        [SerializeField]
        GameObject testModePanel;
        [SerializeField]
        TMP_Dropdown mode;
        public TMP_Dropdown type;
        [SerializeField]
        List<TMP_Dropdown> GestureDatabasesDropdowns;
        [SerializeField]
        GameObject CreateDatabasePopup;
        private List<IGestureDatabase> gestureDatabases;
        [SerializeField]
        RawImage createModeImage;
        [SerializeField]
        TMP_InputField gestureName;
        [SerializeField]
        IGesture tempGesture;
        [SerializeField]
        RawImage inspectModeImage;
        [SerializeField]
        TextMeshProUGUI inspectModeGestureName;
        [SerializeField]
        TextMeshProUGUI currentIdInInspectModeText;
        [SerializeField]
        RawImage DrawedGestureImage;
        [SerializeField]
        RawImage RecognizedGestureImage;
        [SerializeField]
        TextMeshProUGUI DrawedGestureName;
        [SerializeField]
        TextMeshProUGUI RecognizedGestureName;
        private GameObject tempCreateDatabasePopup;
        [SerializeField]
        Texture2D noVisualisationOfGesture;
        [SerializeField]
        private LineRenderer vectorGestureLine;
        [SerializeField]
        private float tolerance;

        int currentIdInInspectMode;

        private void ClearUI()
        {
            createModeImage.texture = null;
            gestureName.text = "";
            tempGesture = null;
            inspectModeImage.texture = null;
            inspectModeGestureName.text = "Gesture Name";
            DrawedGestureImage.texture = null;
            RecognizedGestureImage.texture = null;
            DrawedGestureName.text = "Gesture";
            RecognizedGestureName.text = "Gesture";
        }
        private void Start()
        {
            foreach (var item in GestureDatabasesDropdowns)
            {
                item.onValueChanged.AddListener(OnGestureDatabaseDropdown);
            }
            GestureManager.Instance.OnCreateGesture.AddListener(CreateGesture);
            GestureManager.Instance.OnCreateGesture.AddListener(TestMode);
        }
        private void OnTypeChange(Int32 value)
        {
            if (mode.value == 0 || type.value == 0)
            {
                DisableAllPanels();
                return;
            }
            InitGestureDatabaseDropdown();
            ClearUI();
        }
        private void OnModeChange(Int32 value)
        {
            if (mode.value == 0 || type.value == 0)
            {
                DisableAllPanels();
                return;
            }
            switch (value)
            {
                case 1:
                    SetCreateMode();
                    break;
                case 2:
                    SetInspectMode();
                    break;
                case 3:
                    SetTestMode();
                    break;
                default:
                    break;
            }
        }
        private void SetCreateMode()
        {
            DisableAllPanels();
            createModePanel.SetActive(true);
        }
        private void SetInspectMode()
        {
            DisableAllPanels();
            inspectModePanel.SetActive(true);
            LoadToInspectDatabasePanel(-100);
        }
        private void SetTestMode()
        {
            DisableAllPanels();
            testModePanel.SetActive(true);
        }
        private void DisableAllPanels()
        {
            createModePanel.SetActive(false);
            inspectModePanel.SetActive(false);
            testModePanel.SetActive(false);
        }
        private void OnGestureDatabaseDropdown(Int32 value)
        {
            foreach (var item in GestureDatabasesDropdowns)
            {
                item.value = value;
            }
            DatabaseDropdown(value);
        }
        private void DatabaseDropdown(int value)
        {
            switch (value)
            {
                case 0:
                    GestureManager.Instance.gestureDatabase = null;
                    break;
                case 1:
                    if (tempCreateDatabasePopup == null) tempCreateDatabasePopup = Instantiate(CreateDatabasePopup, transform);
                    break;
                default:
                    GestureManager.Instance.gestureDatabase = gestureDatabases.Find(x => x.databaseName.Equals(GestureDatabasesDropdowns[0].options[value].text));
                    break;
            }
        }
        private void FillGestureDatabaseDropdowns(List<TMP_Dropdown> gestureDatabaseDropdowns, List<IGestureDatabase> gestureDatabases)
        {
            foreach (var item in gestureDatabaseDropdowns)
            {
                item.options.Clear();
                item.options.Add(new TMP_Dropdown.OptionData("None"));
                item.options.Add(new TMP_Dropdown.OptionData("Create Database"));
                foreach (var gestureDatabase in gestureDatabases)
                {
                    item.options.Add(new TMP_Dropdown.OptionData(gestureDatabase.databaseName));
                }
            }
        }
        private void CreateGesture(IGesture gesture)
        {
            if (!createModePanel.activeSelf) return;
            if (type.value == 1)
            {
                ImageGesture imageGesture = (ImageGesture)gesture;
                createModeImage.texture = imageGesture.gestureImage;
                tempGesture = imageGesture;
            }
            else if (type.value == 2)
            {
                VectorGesture vectorGesture = (VectorGesture)gesture;
                tempGesture = vectorGesture;
                VectorVisualization(vectorGesture);
            }
        }
        private void VectorVisualization(VectorGesture gesture)
        {
            vectorGestureLine.positionCount = gesture.vectors.Length;
            List<Vector2> points = new List<Vector2>();
            Vector2 offset = new Vector2(1.875F, 0.816F);
            foreach (var item in gesture.vectors)
            {
                points.Add(points.LastOrDefault() + (item * GestureManager.Instance.pointDistanceOnVector));
            }
            List<Vector3> output = new List<Vector3>();
            for (int i = 0; i < points.Count; i++)
            {
                output.Add(points[i] + offset);
            }
            vectorGestureLine.SetPositions(output.ToArray());
        }
        private void LoadToInspectDatabasePanel(int idChangeValue)
        {
            if (GestureManager.Instance.gestureDatabase == null || GestureManager.Instance.gestureDatabase.gestures.Count == 0)
            {
                ClearUI();
                return;
            }
            int gestureCount = GestureManager.Instance.gestureDatabase.gestures.Count - 1;
            currentIdInInspectMode += idChangeValue;
            if (currentIdInInspectMode < 0 || currentIdInInspectMode > gestureCount) currentIdInInspectMode = 0;
            if (type.value == 1)
            {
                ImageGesture gesture = (ImageGesture)GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
                inspectModeGestureName.text = gesture.gestureName;
                currentIdInInspectModeText.text = currentIdInInspectMode.ToString();
                inspectModeImage.texture = gesture.gestureImage;
            }
            if (type.value == 2)
            {
                VectorGesture gesture = (VectorGesture)GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
                inspectModeGestureName.text = gesture.gestureName;
                currentIdInInspectModeText.text = currentIdInInspectMode.ToString();
                inspectModeImage.texture = noVisualisationOfGesture;

            }
        }
        private void TestMode(IGesture gesture)
        {
            if (!testModePanel.activeSelf) return;
            List<RecognizeOutput> output = GestureManager.Instance.RecognizeGesture(gesture);
            if (type.value == 1)
            {
                ImageGesture imageGesture = (ImageGesture)gesture;
                ImageGesture gestureWithTheHighestProbability = (ImageGesture)output[0].recognizedGesture;

                RecognizedGestureName.text = gestureWithTheHighestProbability.gestureName;
                RecognizedGestureImage.texture = gestureWithTheHighestProbability.gestureImage;

                DrawedGestureName.text = imageGesture.gestureName;
                DrawedGestureImage.texture = imageGesture.gestureImage;
                Debug.Log("Propability:" + output[0].probability);
            }
            else if (type.value == 2)
            {
                VectorGesture vectorGesture = (VectorGesture)gesture;
                VectorGesture gestureWithTheHighestProbability = (VectorGesture)output[0].recognizedGesture;

                RecognizedGestureName.text = gestureWithTheHighestProbability.gestureName;
                RecognizedGestureImage.texture = noVisualisationOfGesture;

                DrawedGestureImage.texture = noVisualisationOfGesture;
                Debug.Log("Propability:" + output[0].probability);
            }
        }
        /// <summary>
        /// Activates when value changes in dropdown in Setting Panel 
        /// </summary>
        /// <param name="value">New value in last changed dropdown</param>
        public void OnSettingsChange(Int32 value)
        {
            OnModeChange(mode.value);
            OnTypeChange(type.value);
        }
        /// <summary>
        /// Fill dropdowns with gesture databases 
        /// </summary>
        public void InitGestureDatabaseDropdown()
        {
            if (type.value == 1)
            {
                gestureDatabases = new List<IGestureDatabase>(GestureManager.Instance.LoadImageDatabase());
            }
            if (type.value == 2)
            {
                gestureDatabases = new List<IGestureDatabase>(GestureManager.Instance.LoadVectorDatabase());
            }
            FillGestureDatabaseDropdowns(GestureDatabasesDropdowns, gestureDatabases);
        }
        /// <summary>
        /// Activates on click SaveGesture button, saves last created gesture and add it to database
        /// </summary>
        public void SaveGesture()
        {
            tempGesture.gestureData.gestureName = gestureName.text;
            GestureManager.Instance.AddGestureToDatabase(tempGesture);
            tempGesture = null;
        }
        /// <summary>
        /// Activates on click RemoveGesture button, remove gesture from database and delete files
        /// </summary>
        public void RemoveGesture()
        {
            if (GestureManager.Instance.gestureDatabase.gestures.Count == 0) return;
            IGesture gesture = GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
            GestureManager.Instance.RemoveGestureFromDatabase(gesture);
            LoadToInspectDatabasePanel(-1);//LoadPrevGesture
        }
    }
}
