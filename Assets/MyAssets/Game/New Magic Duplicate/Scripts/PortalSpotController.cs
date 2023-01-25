using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalSpotController : MonoBehaviour
{
    public bool IsPowered;
    public UnityEvent statusChanged = new UnityEvent();
    List<Collider> collidersInCollider= new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SpellTarget"))
        {
            collidersInCollider.Add(other);
            if (collidersInCollider.Count > 0) IsPowered = true;
            statusChanged.Invoke();
            other.GetComponent<Light>().color = Color.green;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpellTarget"))
        {
            collidersInCollider.Remove(other);
            if (collidersInCollider.Count == 0) IsPowered =false;
            statusChanged.Invoke();
            other.GetComponent<Light>().color = Color.red;
        }
    }
}
