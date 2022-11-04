using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CreateDatabaseController : MonoBehaviour
{
    string savePath = "Assets/Resources/SavedGestures";
    [SerializeField]
    private TMP_InputField inputFieldDatabaseName;
    public void CreateDatabase()
    {
        GestureManager.Instance.CreateDatabase(savePath, inputFieldDatabaseName.text);
        Destroy(gameObject);
    }
}
