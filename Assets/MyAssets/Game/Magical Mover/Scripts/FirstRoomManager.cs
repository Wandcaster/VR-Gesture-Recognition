using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class FirstRoomManager : MonoBehaviour
{
    [SerializeField] List<BoxPlaceController> boxs;
    [SerializeField] Animation doorOpenAnim;
    private void Start()
    {
        foreach (var box in boxs)
        {
            box.boxWasPlaced.AddListener(CheckBoxs);
        }
    }
    public void CheckBoxs()
    {
        boxs.RemoveAt(0);
        if(boxs.Count == 0 ) { ActiveDoor(); };
    }
    private void ActiveDoor()
    {
        doorOpenAnim.Play();
    }
    
}
