using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GestureData", menuName = "GestureManager/GestureData", order = 1)]
namespace VRGesureRecognition
{
    public class ImageGestureData : IGestureData
    {
        public List<Vector2> points;
        public Texture2D gestureImage;
    }
}