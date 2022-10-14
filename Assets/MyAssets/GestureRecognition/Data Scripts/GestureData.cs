using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureData", menuName = "GestureManager/GestureData", order = 1)]
public class GestureData : ScriptableObject
{
    public int ID;
    public string gestureName="Gesture";
    public List<Vector2> points;
    public Texture2D gestureImage;
    

}
