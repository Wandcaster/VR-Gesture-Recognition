using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGesureRecognition;

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
    string gestureName;
   
    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        if (recognizeOutputs[0].recognizedGesture.gestureName != gestureName) return;
        spell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        Destroy(spell, lifeTime);
    }
}
