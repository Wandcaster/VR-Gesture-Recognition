using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGesureRecognition;

public class Duplication : MonoBehaviour
{
    [SerializeField] private string gestureName;
    [SerializeField] private WandModel wandModel;
    [SerializeField] private float recognitionThreshold;
    [SerializeField] private Vector3 positionOffset;
    public bool CastDupliaction(List<RecognizeOutput> recognizeOutputs)
    {
       if (recognizeOutputs[0].recognizedGesture.gestureName.Equals(gestureName) && recognizeOutputs[0].probability < recognitionThreshold)
        {
            Transform target = wandModel.colliders[0].transform;
            GameObject temp=Instantiate(wandModel.colliders[0].gameObject,positionOffset+ target.transform.position, target.transform.rotation );
            temp.transform.localScale = target.lossyScale;
            Destroy(temp.gameObject.GetComponent<ConfigurableJoint>());
            return true;
        }
        return false;
    }
}
