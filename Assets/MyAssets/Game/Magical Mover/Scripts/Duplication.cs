using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGesureRecognition;

public class Duplication : MonoBehaviour
{
    [SerializeField] private string gestureName;
    [SerializeField] private WandModel wandModel;
    [SerializeField] private float recognitionThreshold;

    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        Debug.Log(recognizeOutputs[0].recognizedGesture.gestureName);
        if (recognizeOutputs[0].recognizedGesture.gestureName.Equals(gestureName) && recognizeOutputs[0].probability < recognitionThreshold)
        {
            GameObject temp=Instantiate(wandModel.colliders[0].gameObject );
            Destroy(temp.gameObject.GetComponent<ConfigurableJoint>());
        }
    }
}
