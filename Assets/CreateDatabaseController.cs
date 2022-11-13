using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CreateDatabaseController : MonoBehaviour
{
    
    [SerializeField]
    private TMP_InputField inputFieldDatabaseName;
    public void CreateDatabase()
    {
        GestureManager.Instance.CreateDatabase(inputFieldDatabaseName.text);
        Destroy(gameObject);
    }
}
