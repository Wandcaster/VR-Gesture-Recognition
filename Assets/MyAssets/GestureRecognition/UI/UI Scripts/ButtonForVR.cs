using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

[RequireComponent(typeof(BoxCollider))]
public class ButtonForVR : VRUI
{
    public override void OnClick(PointerEventArgs e)
    {
        Debug.Log(e.target.name);
        GetComponent<Button>().onClick.Invoke();
    }
}
