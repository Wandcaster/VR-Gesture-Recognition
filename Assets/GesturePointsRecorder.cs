using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GesturePointsRecorder : MonoBehaviour
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
    /// Create temporary gameobject that is use to referece center transform of points, the temporary gameobject is destroyed in 'StopCollectData' function
    /// </summary>
    public IEnumerator StartCollectDataCorutine()
    {
        Vector2 point= new Vector2();
        spaceReferencePoint = new GameObject();
        Transform spaceReferencePointTransform = spaceReferencePoint.transform;
        points.Clear();
        spaceReferencePointTransform.position = trackingPoint.position;
        points.Add(spaceReferencePointTransform.InverseTransformPoint(trackingPoint.position));
        while (true)
        {
            if(spaceReferencePointTransform!=null) point = spaceReferencePointTransform.InverseTransformPoint(trackingPoint.position);
            if (point != points[points.Count - 1]) points.Add(point);
            yield return true;
        }
    }

    /// <summary>
    /// Stop Corutine and return Data
    /// </summary>
    /// <returns>PointData that contain information about points collected in recording gesture </returns>
    public PointsData StopCollectData()
    {
        StopCoroutine(coroutine);
        Destroy(spaceReferencePoint);
        return new PointsData(points, accuracy);
    }
    public void StartCollectData()
    {
        coroutine = StartCoroutine(StartCollectDataCorutine());
    }
}
