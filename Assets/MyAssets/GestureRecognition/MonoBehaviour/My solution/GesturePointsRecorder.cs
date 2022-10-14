using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GesturePointsRecorder :IGesturePointsRecorder
{
    [SerializeField]
    private Transform trackingPoint;
    [SerializeField]
    private List<Vector2> points = new List<Vector2>();
    [SerializeField]
    private int accuracy=1000;
    private GameObject spaceReferencePoint;
    private Coroutine coroutine;

    /// <summary>
    /// Start collect point to the list 'points' every frame, skip point that the same like last recorded point.
    /// </summary>
    public IEnumerator StartCollectDataCorutine()
    {
        points.Clear();
        Vector3 startPosition = trackingPoint.position;
        Vector3 point;
        point = trackingPoint.position;
        point -= startPosition;
        point = Vector3.ProjectOnPlane(point,Player.instance.bodyDirectionGuess);
        point.x = point.x + point.z;
        points.Add(new Vector2(point.x,point.y));

        while (true)
        {
            point = trackingPoint.position;
            point -= startPosition;
            point = Vector3.ProjectOnPlane(point, Player.instance.bodyDirectionGuess);
            point.x = point.x+point.z;
            if(new Vector2(point.x , point.y)!=points[points.Count-1])points.Add(new Vector2(point.x, point.y));
            yield return true;
        }
    }

    /// <summary>
    /// Stop Corutine and return Data
    /// </summary>
    /// <returns>PointData that contain information about points collected in recording gesture </returns>
    public override PointsData StopCollectData()
    {
        StopCoroutine(coroutine);
        Destroy(spaceReferencePoint);
        return new PointsData(points, accuracy);
    }
    public override void StartCollectData()
    {
        coroutine = StartCoroutine(StartCollectDataCorutine());
    }
}
