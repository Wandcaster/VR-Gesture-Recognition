using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxPlaceController : MonoBehaviour
{
    public UnityEvent boxWasPlaced;
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("SpellTarget")) return;
        if (other.attachedRigidbody.velocity == Vector3.zero)
        {
            boxWasPlaced.Invoke();
            boxWasPlaced.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
}
