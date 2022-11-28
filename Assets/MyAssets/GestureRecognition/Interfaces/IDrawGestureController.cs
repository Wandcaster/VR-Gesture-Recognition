using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRGesureRecognition
{
    public abstract class IDrawGestureController : MonoBehaviour
    {
        public abstract Texture2D DrawGesture(PointsData pointsData);
        protected abstract Texture2D BoldLines(Texture2D texture);
        protected abstract Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight);
    }
}
