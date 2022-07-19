using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    [SerializeField]
    public int[,] points;
    [SerializeField]
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        //Gesture gesture=CreateRandomGesture("test");
        //image.sprite = Sprite.Create(gesture.gestureImage, new Rect(0.0f, 0.0f, gesture.gestureImage.width, gesture.gestureImage.height), new Vector2(0.5f, 0.5f), 100);
        Gesture gesture = GetComponent<Gesture>();
        gesture.GetPointsFromTexture();
        image.sprite = Sprite.Create(gesture.gestureImage, new Rect(0.0f, 0.0f, gesture.gestureImage.width, gesture.gestureImage.height), new Vector2(0.5f, 0.5f), 100);
    }

    private Gesture CreateRandomGesture(string GestureName)
    {
        points = new int[Gesture.imageSize, Gesture.imageSize];
        for (int i = 0; i < Gesture.imageSize; i++)
        {
            for (int j = 0; j < Gesture.imageSize; j++)
            {
                int random = Random.Range(0, 2);
                points[i, j] = random;
            }
        }
        Gesture gesture = new Gesture(GestureName, points);
        return gesture;
    }
}
