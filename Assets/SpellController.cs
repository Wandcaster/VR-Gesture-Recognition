using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField]
    float velocityMultipler;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    float scaleMultipler;
    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        GameObject temp = null;
        if (recognizeOutputs[0].recognizedGesture.gestureID == 0)temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        else temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ShootObject(temp);
    }

    private void ShootObject(GameObject temp)
    {
        temp.transform.position = spawnPoint.transform.position;
        temp.transform.rotation = spawnPoint.transform.rotation;
        temp.transform.localScale *= scaleMultipler;

        Rigidbody rigidbody = (Rigidbody)temp.AddComponent(typeof(Rigidbody));
        rigidbody.AddForce(spawnPoint.transform.forward * velocityMultipler);
    }
}
