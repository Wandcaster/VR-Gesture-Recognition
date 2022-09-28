using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureUIController : MonoBehaviour
{
    [SerializeField]
    Transform GestureContainer;
    [SerializeField]
    GameObject GestureUIPrefab;
    public List<GameObject> UIGesture = new List<GameObject>();

    public void AddGestureToUI(Texture2D gestureImage,Gesture gesture)
    {
        UIGesture.Add(Instantiate(GestureUIPrefab, GestureContainer));
        UIGesture[UIGesture.Count - 1].GetComponent<GestureUIItem>().gestureImage.texture = gestureImage;
        UIGesture[UIGesture.Count - 1].GetComponent<GestureUIItem>().gesture = gesture;
    }
}
