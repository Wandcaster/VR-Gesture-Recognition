using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureData", menuName = "GestureManager/GestureData", order = 1)]
public class GestureData : ScriptableObject
{
    public int ID;
    public string gestureName;
    public int[,] points;
    public List<int> pointsInInspector;
    public Texture2D gestureImage;
    [HideInInspector]
    public Texture2D tempGestureImage;
    public void Init()
    {
        tempGestureImage = gestureImage;
    }
    public void InitGestureImage()
    {
        gestureImage = tempGestureImage;
    }

}
