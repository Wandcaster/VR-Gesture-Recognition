using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GestureUIController : MonoBehaviour
{
    [SerializeField]
    Transform GestureContainer;
    [SerializeField]
    Transform GestureDatabaseContainer;
    [SerializeField]
    GameObject GestureUIPrefab;
    [SerializeField]
    GameObject GestureDatabaseUIPrefab;
    [SerializeField]
    public TMP_InputField gestureName;
    public List<GameObject> UIGesture = new List<GameObject>();

    public void AddGestureToUI(Texture2D gestureImage,Gesture gesture)
    {
        UIGesture.Add(Instantiate(GestureUIPrefab, GestureContainer));
        UIGesture[UIGesture.Count - 1].GetComponent<GestureUIItem>().gestureImage.texture = gestureImage;
        UIGesture[UIGesture.Count - 1].GetComponent<GestureUIItem>().gesture = gesture;
        UIGesture[UIGesture.Count - 1].GetComponent<GestureUIItem>().gestureName.text = gesture.gestureName;
    }
    //public void SaveDatabase(string path)
    //{
    //    GestureManager.Instance.SaveDatabase(path);
    //}
    public void LoadDatabase(string path)
    {
        for (int i = 0; i < GestureDatabaseContainer.childCount; i++)
        {
            Destroy(GestureDatabaseContainer.GetChild(i).gameObject);
        }
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        foreach (var directory in dirInfo.GetDirectories())
        {
            Debug.Log(path + "/" + directory.Name);
            AddDatabaseFromPath(path + "/" + directory.Name + "/");
        }
    }
    private void AddDatabaseFromPath(string path)
    {
        path = path.Remove(0, 17);
        foreach (var database in Resources.LoadAll<GestureDatabase>(path))
        {
            GameObject temp = Instantiate(GestureDatabaseUIPrefab, GestureDatabaseContainer);
            temp.GetComponent<GestureDatabaseUI>().gestureDatabase = database;
            temp.GetComponent<GestureDatabaseUI>().databaseName.text = database.databaseName;
        }
    }
    public void ClearUI()
    {
        foreach (var item in UIGesture)
        {
            Destroy(item);
        }
        UIGesture.Clear();
    }
    //public void SetGestureDatabaseName()
    //{
    //    GestureManager.Instance.SetGestureDatabaseName(gestureName.text);
    //}
}
