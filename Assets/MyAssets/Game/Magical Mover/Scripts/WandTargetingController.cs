using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WandTargetingController : MonoBehaviour
{
    public WandModel wandModel;
    public WandView wandView;
    public List<Collider> colliders
    {
        get { return wandModel.colliders; }
        set { wandModel.colliders = value; }
    }
    private void Start()
    {
        wandModel=GetComponent<WandModel>();
        wandView= GetComponent<WandView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SpellTarget")) return;
        if (!colliders.Contains(other)) { colliders.Add(other); }
        SortColliders(colliders);
        ColorNerestCollider(colliders);

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("SpellTarget")) return;
        colliders.Remove(other);
        SortColliders(colliders);
        ColorNerestCollider(colliders);
        wandView.ChangeTargetColor(other.gameObject, wandModel.targetNormalColor);
    }
    private void SortColliders(List<Collider> colliders)
    {
        this.colliders= colliders.OrderBy(x=>Vector3.Distance(transform.position,x.transform.position)).ToList();
    }
    private void ColorNerestCollider(List<Collider> colliders)
    {
        if(colliders.Count < 1)  return; 
        foreach (var collider in colliders)
        {
            wandView.ChangeTargetColor(collider.gameObject, wandModel.targetNormalColor);
        }
        wandView.ChangeTargetColor(colliders[0].gameObject, wandModel.targetHoverColor);
    }
}

