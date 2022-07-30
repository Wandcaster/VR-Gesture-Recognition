using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureManagerTests : MonoBehaviour
{
    [SerializeField]
    GestureManager gestureManager;
    [SerializeField]
    Gesture gestureToRecognize;
    [SerializeField]
    private Image image;
    public static Image tempImage;
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown("x"))
        {

            List<RecognizeOutput> gestureOutput = gestureManager.RecognizeGesture(gestureToRecognize);
            foreach (var gesture in gestureOutput)
            {
                Debug.Log(gesture.recognizedGesture.gestureName + " Probability:" + gesture.probability);
            }
            Gesture result = gestureOutput[0].recognizedGesture;
            image.sprite = Sprite.Create(result.gestureImage, new Rect(0.0f, 0.0f, result.gestureImage.width, result.gestureImage.height), new Vector2(0.5f, 0.5f), 100);
        }
    }
    private void Start()
    {
        tempImage = image;
    }
}
