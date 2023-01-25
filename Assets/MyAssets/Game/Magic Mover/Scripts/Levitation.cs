using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGesureRecognition;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Levitation : MonoBehaviour
{
    public float upwardForce = -0.12f; // Adjust this value to control the average upward force
    public float forceVariance = 2.67f; // Adjust this value to control the range of random forces
    public float wandMultiplier;
    [SerializeField] public string gestureName;
    private Rigidbody rb;
    public WandModel wandModel;
    bool isActive;
    [SerializeField] private float recognitionThreshold;
    [SerializeField] private SteamVR_Action_Single spellForce;
    private void Active()
    {
        if (wandModel.colliders.Count < 1) return;
        rb = wandModel.colliders[0].attachedRigidbody;
        isActive = true;
    }
    private void Deactive()
    {
        if (wandModel.colliders.Count>0)
        {
            rb = wandModel.colliders[0].attachedRigidbody;
            isActive = false;
        }
        else
        {
            rb = null;
            isActive = false;
        }
    }

    //dzwiek poruszania obiektu
    void FixedUpdate()
    {
        if (rb == null) return;
        float randomForce = Random.Range(-forceVariance, forceVariance)+upwardForce;
        float forceUp = Physics.gravity.magnitude;
        Vector3 forceFromMovment = wandModel.rateOfChangePosition * spellForce.axis*wandMultiplier;
        if (spellForce.axis > 0)
        {
            wandModel.GetComponent<Interactable>().attachedToHand.TriggerHapticPulse((ushort)(spellForce.axis * wandMultiplier));
            rb.AddForce(Vector3.up * (randomForce + forceUp) + forceFromMovment);
        }
        else
        {
            rb.AddForce(-rb.velocity / 100);
        }
    }
    public bool CastLevitation(List<RecognizeOutput>recognizeOutputs)
    {
        if (recognizeOutputs[0].recognizedGesture.gestureName.Equals(gestureName) && recognizeOutputs[0].probability< recognitionThreshold)
        {
            if (isActive) Deactive();
            else Active();
            return true;
        }
        return false;
    }

}
