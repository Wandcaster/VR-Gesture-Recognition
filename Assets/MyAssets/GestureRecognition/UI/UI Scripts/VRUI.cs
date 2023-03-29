using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
[RequireComponent(typeof(BoxCollider))]
public class VRUI :MonoBehaviour, IOnClick
{
    private SteamVR_LaserPointer steamVR_Laser;
    public void Awake()
    {
        GetComponent<BoxCollider>().size = Vector3.zero;
        StartCoroutine(SetColliderSize());
    }
    private void Start()
    {
        steamVR_Laser = FindObjectOfType<SteamVR_LaserPointer>();
        steamVR_Laser.PointerClick += OnClick;
    }
    private IEnumerator SetColliderSize()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        while (boxCollider.size.x == 0 || boxCollider.size.y == 0)
        {
            Vector2 reactSize = GetComponent<RectTransform>().rect.size;
            boxCollider.size = new Vector3(reactSize.x, reactSize.y, 0);
            yield return true;
        }
        yield return true;
    }

    public virtual void OnClick(object sender,PointerEventArgs e)
    {
        throw new System.NotImplementedException();
    }
    private void OnDestroy()
    {
        if(steamVR_Laser != null) steamVR_Laser.PointerClick -= OnClick;
    }
}
