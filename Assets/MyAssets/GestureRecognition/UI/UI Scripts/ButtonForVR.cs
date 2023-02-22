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
    public override void OnClick(object sender, PointerEventArgs e)
    {
        if (e.target != gameObject.transform) return;
        GetComponent<Button>().onClick.Invoke();
    }
}
