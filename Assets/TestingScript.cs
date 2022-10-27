using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField]
    List<Vector2> points;
    [SerializeField]
    int accuracy;
    PointsData gesture0;
    Gesture tempGesture;
    [SerializeField]
    private IDrawGestureController drawController;
    [SerializeField]
    private IGestureRecognizer gestureRecognizer;
    public GestureDatabase gestureDatabase;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            gesture0 = new PointsData(points, accuracy);
            Texture2D gestureImage = drawController.DrawGesture(gesture0);
            tempGesture = new Gesture("Gesture", gestureImage, gesture0.rawPoints);
            List<RecognizeOutput> output = gestureRecognizer.RecognizeGesture(tempGesture, ref gestureDatabase.gestures);
            Debug.Log(output[0].recognizedGesture.gestureName + " " + output[0].probability);
            Debug.Break();
        }
    }
}
