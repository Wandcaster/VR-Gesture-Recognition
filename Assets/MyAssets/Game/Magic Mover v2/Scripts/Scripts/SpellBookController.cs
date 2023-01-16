using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGesureRecognition;

public class SpellBookController : MonoBehaviour
{
    [SerializeField]
    Levitation levitation;
    [SerializeField]
    Duplication duplication;
    [SerializeField]
    MagicRopeController magicRope;
    [SerializeField] AudioClip OnCast;
    [SerializeField] AudioClip OnFailCast;
    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        if (levitation.CastLevitation(recognizeOutputs))
        {
            levitation.wandModel.audioSource.clip = OnCast;
            levitation.wandModel.audioSource.Play();
            return;
        }
        if (duplication.CastDupliaction(recognizeOutputs))
        {
            levitation.wandModel.audioSource.clip = OnCast;
            levitation.wandModel.audioSource.Play();
            return;
        }
        if (magicRope.CastMagicRope(recognizeOutputs))
        {
            levitation.wandModel.audioSource.clip = OnCast;
            levitation.wandModel.audioSource.Play();
            return;
        }
        levitation.wandModel.audioSource.clip = OnFailCast;
        levitation.wandModel.audioSource.Play();
    }

}
