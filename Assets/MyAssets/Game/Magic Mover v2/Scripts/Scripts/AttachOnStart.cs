using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;

public class AttachOnStart : MonoBehaviour
{
    private Valve.VR.InteractionSystem.Hand hand;
    public GameObject target;
    [SerializeField] private Transform attachOffset;
    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponentInParent<Valve.VR.InteractionSystem.Hand>();
    }
    void Update()
    {
        if (target != null)
        {
            if (hand.isActive && hand.isPoseValid)  
            {

                target.SetActive(true);
                hand.AttachObject(target, GrabTypes.Grip,attachmentOffset:attachOffset);
                // If the player's scale has been changed the object to attach will be the wrong size.
                // To fix this we change the object's scale back to its original, pre-attach scale.
                target.transform.localScale = target.transform.localScale;
            }
        }
    }
}
