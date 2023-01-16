using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRGesureRecognition;

public class WandModel : MonoBehaviour
{
    public Color targetHoverColor;
    public Color targetNormalColor;
    public Collider targetHoverCollider;
    public List<Collider> colliders = new List<Collider>();
    public Vector3 rateOfChangePosition;
    public Transform tip;
    public AudioSource audioSource;
    public SteamVR_Action_Boolean isRecording;

    private void Start()
    {
        isRecording = GestureManager.Instance.isRecording;
    }

}
