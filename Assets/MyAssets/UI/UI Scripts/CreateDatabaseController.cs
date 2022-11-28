using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using VRGesureRecognition;

public class CreateDatabaseController : MonoBehaviour
{
    
    [SerializeField]
    private TMP_InputField inputFieldDatabaseName;
    public void CreateDatabase()
    {
        UIController gestureUI = FindObjectOfType<UIController>();
        GestureManager.Instance.CreateDatabase(inputFieldDatabaseName.text, (GestureType)gestureUI.type.value);
        Destroy(gameObject);
    }
}
