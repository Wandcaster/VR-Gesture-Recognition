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
        points.Clear();
        Vector3 startPosition = trackingPoint.position;
        Vector3 point;
        point = trackingPoint.position;
        point -= startPosition;
        point = Vector3.ProjectOnPlane(point,Camera.main.transform.forward);
        point.x = point.x + point.z;
        points.Add(new Vector2(point.x,point.y));

        while (true)
        {
            point = trackingPoint.position;
            point -= startPosition;
            point = Vector3.ProjectOnPlane(point,Camera.main.transform.forward);
            point.x = point.x+point.z;
            if(new Vector2(point.x , point.y)!=points[points.Count-1])points.Add(new Vector2(point.x, point.y));
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Ray(Camera.main.transform.position, Camera.main.transform.right));
    }
}
