using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    public void InitGestureDatabaseDropdown() 
    {
        if(type.value==1)
        {
            gestureDatabases = new List<IGestureDatabase>(GestureManager.Instance.LoadImageDatabase("Assets/Resources/SavedGestures"));
        }
        if(type.value==2)
        {
            gestureDatabases = new List<IGestureDatabase>(GestureManager.Instance.LoadVectorDatabase("Assets/Resources/SavedGestures"));
        }
        FillGestureDatabaseDropdowns(GestureDatabasesDropdowns, gestureDatabases);
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
    public void OnSettingsChange(Int32 value)
    {
        OnModeChange(mode.value);
        OnTypeChange(type.value);
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
    public void DisableAllPanels()
    {
        createModePanel.SetActive(false);
        inspectModePanel.SetActive(false);
        testModePanel.SetActive(false);
    }
    public void OnGestureDatabaseDropdown(Int32 value)
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
                if(tempCreateDatabasePopup==null) tempCreateDatabasePopup=Instantiate(CreateDatabasePopup, transform);
                break;
            default:
                GestureManager.Instance.gestureDatabase = gestureDatabases.Find(x => x.databaseName.Equals(GestureDatabasesDropdowns[0].options[value].text));
                break;
        }
    }
    private void FillGestureDatabaseDropdowns(List<TMP_Dropdown> gestureDatabaseDropdowns,List<IGestureDatabase>gestureDatabases)
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
    public void CreateGesture(IGesture gesture)
    {
        if (!createModePanel.activeSelf) return;
        if (type.value == 1)
        {
            ImageGesture imageGesture=(ImageGesture) gesture;
            createModeImage.texture = imageGesture.gestureImage;
            tempGesture = imageGesture;
        }
        else if(type.value==2)
        {
            VectorGesture vectorGesture = (VectorGesture)gesture;
            tempGesture = vectorGesture;
        }
    }
    public void SaveGesture() 
    {
        tempGesture.gestureData.gestureName = gestureName.text;
        GestureManager.Instance.AddGestureToDatabase(tempGesture);
        tempGesture = null;
    }
    public void LoadToInspectDatabasePanel(int idChangeValue)
    {
        if (GestureManager.Instance.gestureDatabase == null || GestureManager.Instance.gestureDatabase.gestures.Count == 0)
        {
            ClearUI();
            return;
        }
        int gestureCount = GestureManager.Instance.gestureDatabase.gestures.Count-1;
        currentIdInInspectMode += idChangeValue;
        if (currentIdInInspectMode < 0 || currentIdInInspectMode > gestureCount) currentIdInInspectMode=0;
        if (type.value == 1)
        {
            ImageGesture gesture = (ImageGesture)GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
            inspectModeGestureName.text = gesture.gestureName;
            currentIdInInspectModeText.text = currentIdInInspectMode.ToString();
            inspectModeImage.texture = gesture.gestureImage;
        }
        if(type.value==2)
        {
            VectorGesture gesture = (VectorGesture)GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
            inspectModeGestureName.text = gesture.gestureName;
            currentIdInInspectModeText.text = currentIdInInspectMode.ToString();
            inspectModeImage.texture = noVisualisationOfGesture;

        }
    }
    public void RemoveGesture()
    {
        if (GestureManager.Instance.gestureDatabase.gestures.Count == 0) return;
        IGesture gesture = GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
        GestureManager.Instance.RemoveGestureFromDatabase(gesture);
        LoadToInspectDatabasePanel(-1);//LoadPrevGesture
    }
    private void TestMode(IGesture gesture)
    {
        if (!testModePanel.activeSelf) return;
        List<RecognizeOutput> output=null;
        if (type.value == 1)
        {
            ImageGesture imageGesture = (ImageGesture)gesture;
            output = GestureManager.Instance.gestureRecognizer.RecognizeGesture(imageGesture, GestureManager.Instance.gestureDatabase.gestures);
            ImageGesture gestureWithTheHighestProbability = (ImageGesture)output[0].recognizedGesture;
            RecognizedGestureName.text = gestureWithTheHighestProbability.gestureName;
            RecognizedGestureImage.texture = gestureWithTheHighestProbability.gestureImage;

            DrawedGestureName.text = imageGesture.gestureName;
            DrawedGestureImage.texture = imageGesture.gestureImage;
            Debug.Log("Propability:" + output[0].probability);
        }
        else if(type.value==2)
        {
            VectorGesture imageGesture = (VectorGesture)gesture;
            output = GestureManager.Instance.vectorGestureRecognizer.RecognizeGesture(imageGesture, GestureManager.Instance.gestureDatabase.gestures);
            VectorGesture gestureWithTheHighestProbability = (VectorGesture)output[0].recognizedGesture;
            RecognizedGestureName.text = gestureWithTheHighestProbability.gestureName;
            RecognizedGestureImage.texture = noVisualisationOfGesture;
            DrawedGestureImage.texture = noVisualisationOfGesture;
            Debug.Log("Propability:" + output[0].probability);
        }
        GestureManager.Instance.OnRecognition.Invoke(output);
    }
}
