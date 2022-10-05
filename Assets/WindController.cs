using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WindController : MonoBehaviour
{
    [SerializeField]
    private float ForceMultipler;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.parent = transform;
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Throwable>().onPickUp.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            other.transform.parent = null;
            other.GetComponent<Rigidbody>().useGravity = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            other.GetComponent<Rigidbody>().AddForce((transform.position - other.transform.position).normalized * ForceMultipler,ForceMode.VelocityChange);
        }
    }
    private void OnDestroy()
    {
        foreach (var item in GetComponentsInChildren<Transform>())
        {
            if (item.GetInstanceID() != transform.GetInstanceID())
            {
                item.parent = null;
                item.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
