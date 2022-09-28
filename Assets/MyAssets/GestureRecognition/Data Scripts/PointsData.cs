using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointsData
{
    public List<Vector2> rawPoints { get; }
    public int[,] pointsAfterTransform { get; }
    public float maxX { get; }
    public float minX { get; }
    public float maxY { get; }
    public float minY { get; }
    public float width { get; }
    public float height { get; }
    public Vector2 minPoint { get; }
    public Vector2Int expectedSize { get; }

    public PointsData(List<Vector2> points, int accuracy)
    {
        maxX = points.Max(x => x.x);
        minX = points.Min(x => x.x);
        maxY = points.Max(y => y.y);
        minY = points.Min(y => y.y);
        width = Mathf.Abs(maxX) + Mathf.Abs(minX);
        height = Mathf.Abs(maxY) + Mathf.Abs(minY);
        minPoint = new Vector2(minX * accuracy , minY*accuracy);
        expectedSize = new Vector2Int((int)(width * accuracy), (int)(height * accuracy));
        TransformPointOutput transformPointOutput = TransformPoints(points, accuracy);
        pointsAfterTransform = transformPointOutput.outputPoints;
        rawPoints = transformPointOutput.pointsAfterCheck;
    }
    private TransformPointOutput TransformPoints(List<Vector2> points, int accuracy)
    {
        int[,] outputPoints = new int[expectedSize.x, expectedSize.y];
        List<Vector2> newPoints = new List<Vector2>();

        foreach (var item in points)
        {
            Vector2 tempCoordinate = item;

            tempCoordinate *= accuracy;
            tempCoordinate -= minPoint;

            if (checkPoints(tempCoordinate))
            {
                outputPoints[(int)tempCoordinate.x, (int)tempCoordinate.y] = 1;
                newPoints.Add(item);
            }
        }
        return new TransformPointOutput(outputPoints, newPoints);
    }
    private bool checkPoints(Vector2 point)
    {
        if (point.x <= 0) return false;
        if (point.y <= 0) return false;

        if (point.x >= expectedSize.x - 1) return false;
        if (point.y >= expectedSize.y - 1) return false;

        return true;
    }
}
class TransformPointOutput
{
    public int[,] outputPoints { get; }
    public List<Vector2> pointsAfterCheck { get; }
    public TransformPointOutput(int[,] outputPoints, List<Vector2> pointsAfterCheck)
    {
        this.outputPoints = outputPoints;
        this.pointsAfterCheck = pointsAfterCheck;
    }
}

