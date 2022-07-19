using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureData", menuName = "GestureManager/GestureData", order = 1)]
public class GestureData : ScriptableObject
{
    public string gestureName;
    public int[,] points;
    public Texture2D gestureImage;
    public int pointsCount;
}
