using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandModel : MonoBehaviour
{
    public Color targetHoverColor;
    public Color targetNormalColor;
    public Collider targetHoverCollider;
    public List<Collider> colliders = new List<Collider>();
    public Vector3 rateOfChangePosition;
    public Transform tip;
}
