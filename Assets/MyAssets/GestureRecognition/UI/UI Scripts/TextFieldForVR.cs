using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
[RequireComponent(typeof(BoxCollider))]
public class TextFieldForVR : VRUI
{
    public override void OnClick(object sender, PointerEventArgs e)
    {
        if (e.target != gameObject.transform) return;
        Debug.Log(e.target.name);
        GetComponent<TMP_InputField>().Select();
    }
}
