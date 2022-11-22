using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class DropDownForVR : VRUI
{
    public override void OnClick(PointerEventArgs e)
    {
        Debug.Log("Dropdown");
        GetComponent<IPointerClickHandler>().OnPointerClick(new PointerEventData(EventSystem.current));
        foreach (var item in GetComponentsInChildren<Toggle>())
        {
            item.gameObject.AddComponent<ToggleForVR>();
        }
    }
}
