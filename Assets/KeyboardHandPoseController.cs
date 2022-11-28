using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class KeyboardHandPoseController : MonoBehaviour
{
    [SerializeField]
    SteamVR_Skeleton_Poser poser;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().attachedRigidbody.GetComponent<HandCollider>()!=null)
        {
            other.GetComponent<Collider>().attachedRigidbody.GetComponent<HandCollider>().hand.hand.skeleton.BlendToPoser(poser, 0.2F);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().attachedRigidbody.GetComponent<HandCollider>() != null)
        {
            other.GetComponent<Collider>().attachedRigidbody.GetComponent<HandCollider>().hand.hand.skeleton.BlendToSkeleton(0.2F);
        }
    }
    
}
