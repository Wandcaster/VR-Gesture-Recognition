using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class ToggleForVR : VRUI
{
    public override void OnClick(object sender, PointerEventArgs e)
    {
        if (e.target != gameObject.transform) return;
        GetComponent<IPointerClickHandler>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
