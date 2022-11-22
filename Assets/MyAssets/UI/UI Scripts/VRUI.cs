using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

[RequireComponent(typeof(BoxCollider))]
public class VRUI :MonoBehaviour,IOnClick
{
    public void Awake()
    {
        GetComponent<BoxCollider>().size = Vector3.zero;
        StartCoroutine(SetColliderSize());
    }
    private IEnumerator SetColliderSize()
    {
        while (GetComponent<BoxCollider>().size.x == 0 || GetComponent<BoxCollider>().size.y == 0)
        {
            Vector2 reactSize = GetComponent<RectTransform>().rect.size;
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(reactSize.x, reactSize.y, 0);
            Vector3 rectPosition=GetComponent<RectTransform>().rect.position;
            yield return true;
            boxCollider.center = new Vector3(0 , 0, 0);
        }
        yield return true;
    }

    public virtual void OnClick(PointerEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
