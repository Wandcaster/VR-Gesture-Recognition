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
    [SerializeField]
    TMP_Dropdown type;
    [SerializeField]
    List<TMP_Dropdown> GestureDatabasesDropdowns;
    [SerializeField]
    GameObject CreateDatabasePopup;
    private List<GestureDatabase> gestureDatabases;
    [SerializeField]
    RawImage createModeImage;
    [SerializeField]
    TMP_InputField gestureName;
    Gesture tempGesture;
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
                Instantiate(CreateDatabasePopup, transform);
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
    public void CreateGesture(Gesture gesture)
    {
        if (!createModePanel.activeSelf) return;
        createModeImage.texture = gesture.gestureImage; //Po drugim u¿yciu rysuje bzdety
        tempGesture = gesture;
    }
    public void SaveGesture()
    {
        tempGesture.gestureName = gestureName.text;
        GestureManager.Instance.AddGestureToDatabase(tempGesture);
        tempGesture = null;
    }
    public void LoadToInspectDatabasePanel(int idChangeValue)
    {
        if (GestureManager.Instance.gestureDatabase == null) return;
        int gestureCount = GestureManager.Instance.gestureDatabase.gestures.Count-1;
        currentIdInInspectMode += idChangeValue;
        if (currentIdInInspectMode < 0 || currentIdInInspectMode > gestureCount) currentIdInInspectMode=0;
        Gesture gesture = GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
        inspectModeImage.texture = gesture.gestureImage;
        inspectModeGestureName.text = gesture.gestureName;
        currentIdInInspectModeText.text = currentIdInInspectMode.ToString();
    }
    public void RemoveGesture()
    {
        Gesture gesture = GestureManager.Instance.gestureDatabase.gestures[currentIdInInspectMode];
        GestureManager.Instance.RemoveGestureFromDatabase(gesture);
        LoadToInspectDatabasePanel(-1);//LoadPrevGesture
    }
    private void TestMode(Gesture gesture)
    {
        if (!testModePanel.activeSelf) return;
        List<RecognizeOutput> output = GestureManager.Instance.gestureRecognizer.RecognizeGesture(gesture, GestureManager.Instance.gestureDatabase.gestures);
        RecognizedGestureName.text = output[0].recognizedGesture.gestureName;
        RecognizedGestureImage.texture = output[0].recognizedGesture.gestureImage;

        DrawedGestureName.text = gesture.gestureName;
        DrawedGestureImage.texture = gesture.gestureImage;
        Debug.Log("Propability:" + output[0].probability);
    }
}
