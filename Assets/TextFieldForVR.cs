using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
[RequireComponent(typeof(BoxCollider))]
public class TextFieldForVR : MonoBehaviour, IOnClick
{
    public void OnClick(PointerEventArgs e)
    {
        Debug.Log(e.target.name);
        GetComponent<TMP_InputField>().Select();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().size = Vector3.zero;
        StartCoroutine(SetColliderSize());
    }
    private IEnumerator SetColliderSize()
    {
        while (GetComponent<BoxCollider>().size.x == 0 || GetComponent<BoxCollider>().size.y == 0)
        {
            Vector2 reactSize = GetComponent<RectTransform>().rect.size;
            GetComponent<BoxCollider>().size = new Vector3(reactSize.x, reactSize.y, 0);
            yield return true;
        }
        yield return true;
    }
}
