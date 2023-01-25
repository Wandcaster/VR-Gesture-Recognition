using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandView : MonoBehaviour
{
    private WandModel wandModel;
    private void Start()
    {
        wandModel=GetComponent<WandModel>();
    }

    public void ChangeTargetColor(GameObject target, Color color)
    {
        foreach (var material in target.GetComponent<MeshRenderer>().materials)
        {
            material.color = color;
        }
    }

}
