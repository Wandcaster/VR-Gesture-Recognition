using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDrawGestureController:MonoBehaviour
{
    public abstract Texture2D DrawGesture(PointsData pointsData);
    public abstract Texture2D BoldLines(Texture2D texture);
    public abstract Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight);
}
