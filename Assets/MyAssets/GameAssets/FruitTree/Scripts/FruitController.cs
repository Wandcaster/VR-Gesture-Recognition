using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FruitController : MonoBehaviour
{
    [SerializeField]
    bool isGrowing;
    public bool attachedToTree;
    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float StateGrowTime;
    [SerializeField]
    float UpdateStatusWaitTime;
    [Range(0,1)]
    public float growProggres;
    [SerializeField]
    [Range(0, 1)]
    float pickUpTreshold;
    private bool canPickUp=false;
    public FruitSlotController fruitSlot;

    private Material material;
    private Throwable throwable;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().materials[0];
        throwable = GetComponent<Throwable>();
        StartCoroutine(CheckStatus());
        StartCoroutine(UpdateStatus());
    }
    private void ChangeGrowProgress(float value)
    {
        growProggres += value;
        if (growProggres > 1) growProggres = 1;
        if (growProggres < 0) growProggres = 0;
        transform.localScale = new Vector3(growProggres, growProggres, growProggres);
        material.SetFloat("Vector1_4df39215be94416c90310741114feced", growProggres);
    }
    private IEnumerator CheckStatus()
    {
        while(true)
        {
            yield return new WaitForSeconds(UpdateStatusWaitTime);
            if (growProggres == 1) isGrowing = false;
            canPickUp = pickUpTreshold < growProggres ? true : false;
            yield return true;
        }        
    }
    private IEnumerator UpdateStatus()
    {
        while(true)
        {
            if(isGrowing)
            {
                yield return new WaitForSeconds(StateGrowTime);
                ChangeGrowProgress(growSpeed);
            }
            if (canPickUp && throwable == null) throwable.enabled = true;
            else throwable.enabled = false;

            yield return true;
        }   
    }
    public void SetIsGrowing(bool isGrowing)
    {
        this.isGrowing = isGrowing;
    }
    public void ChangeLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
    }
    public void SetAttachedToTree(bool attachedToTree)
    {
        this.attachedToTree = attachedToTree;
    }
    public void DeAttachFromSlot()
    {
        fruitSlot.attachedFruit = null;
    }

}
