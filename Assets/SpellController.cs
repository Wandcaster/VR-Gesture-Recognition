using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject spellPrefab;
    GameObject spell;
    [SerializeField]
    float lifeTime;
    [SerializeField]
    int gestureID;
   
    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        Debug.Log(recognizeOutputs[0].recognizedGesture.gestureID);
        if (recognizeOutputs[0].recognizedGesture.gestureID != gestureID) return;
        spell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        Destroy(spell, lifeTime);
    }
}
