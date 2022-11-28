//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class GestureTextWriter : MonoBehaviour
//{
//    List<TMP_InputField> textFields;
//    void Start()
//    {
//        textFields = new List<TMP_InputField>(FindObjectsOfType<TMP_InputField>());
//    }
//    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
//    {
//        foreach (var item in textFields)
//        {
//            if (item.isFocused)
//            {
//                if (recognizeOutputs[0].recognizedGesture.gestureName == "space")
//                {
//                    item.text += " ";
//                    return;
//                }
//                if (recognizeOutputs[0].recognizedGesture.gestureName == "delete")
//                {
//                    Debug.Log("remove");
//                    item.text=item.text.Remove(item.text.Length-1,1);
//                    return;
//                }
//                item.text += recognizeOutputs[0].recognizedGesture.gestureName;

//            }
//        }


//    }
//}
