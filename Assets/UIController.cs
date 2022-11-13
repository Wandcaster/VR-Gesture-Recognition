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
    private List<GestureDatabase> gestureDatabases;
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

    int currentIdInInspectMode;

    private void Start()
    {
        foreach (var item in GestureDatabasesDropdowns)
        {
            item.onValueChanged.AddListener(OnGestureDatabaseDropdown);
        }
        InitGestureDatabaseDropdown();
        GestureManager.Instance.OnCreateGesture.AddListener(CreateGesture);
        GestureManager.Instance.OnCreateGesture.AddListener(TestMode);
    }
    public void InitGestureDatabaseDropdown() 
    {
        gestureDatabases = GestureManager.Instance.LoadDatabase("Assets/Resources/SavedGestures");
        FillGestureDatabaseDropdowns(GestureDatabasesDropdowns, gestureDatabases);
    }
    private void OnTypeChange(Int32 value)
    {
        if (mode.value == 0 || type.value == 0)
        {
            DisableAllPanels();
            return;
        }
        switch (value)
        {
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
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

    private void FillGestureDatabaseDropdowns(List<TMP_Dropdown> gestureDatabaseDropdowns,List<GestureDatabase>gestureDatabases)
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
            Gesture imageGesture=(Gesture) gesture;
            createModeImage.texture = imageGesture.gestureImage;
            tempGesture = imageGesture;
        }
        else if(type.value==2)
        {
            VectorGesture vectorGesture = (VectorGesture)gesture;
            tempGesture = vectorGesture;
        }
    }
    public void SaveGesture() //Do poprawy
    {
        tempGesture.gestureData.gestureName = gestureName.text;
        GestureManager.Instance.AddGestureToDatabase(tempGesture);
        tempGesture = null;
    }
    public void LoadToInspectDatabasePanel(int idChangeValue)
    {
        if (GestureManager.Instance.gestureDatabase == null) return;
        int gestureCount = GestureManager.Instance.gestureDatabase.gestures.Count-1;
        currentIdInInspectMode += idChangeValue;
        if (currentIdInInspectMode < 0 || currentIdInInspectMode > gestureCount) currentIdInInspectMode=0;

        IGesture gesture = GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
        inspectModeGestureName.text = gesture.gestureName;
        currentIdInInspectModeText.text = currentIdInInspectMode.ToString();
    }
    public void RemoveGesture()
    {
        IGesture gesture = GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
        GestureManager.Instance.RemoveGestureFromDatabase(gesture);
        LoadToInspectDatabasePanel(-1);//LoadPrevGesture
    }
    private void TestMode(IGesture gesture)
    {
        if (!testModePanel.activeSelf) return;
        if (type.value == 1)
        {
            Gesture imageGesture = (Gesture)gesture;
            List<RecognizeOutput> output = GestureManager.Instance.gestureRecognizer.RecognizeGesture(imageGesture, GestureManager.Instance.gestureDatabase.gestures);
            Gesture gestureWithTheHighestProbability = (Gesture)output[0].recognizedGesture;
            RecognizedGestureName.text = gestureWithTheHighestProbability.gestureName;
            RecognizedGestureImage.texture = gestureWithTheHighestProbability.gestureImage;

            DrawedGestureName.text = imageGesture.gestureName;
            DrawedGestureImage.texture = imageGesture.gestureImage;
            Debug.Log("Propability:" + output[0].probability);
        }
        else if(type.value==2)
        {
            //List<RecognizeOutput> output = GestureManager.Instance.vectorGestureRecognizer.RecognizeGesture((VectorGesture)gesture,GestureManager.Instance.gestureDatabase.gestures);
            //VectorGesture gestureWithTheHighestProbability = (VectorGesture)output[0].recognizedGesture;
            //RecognizedGestureName.text = gestureWithTheHighestProbability.gestureName;
            //Debug.Log("Propability:" + output[0].probability);
            //Dodaæ vizualizacjê
        }

    }
}
