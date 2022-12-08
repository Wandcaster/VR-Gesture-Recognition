using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using VRGesureRecognition;

public class RecognitionPerformance 
{
    GameObject gameobject;
    [SetUp]
    public void InitData()
    {
        gameobject = new GameObject();
    }
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator ImageRecognition([ValueSource("ImageGesturePath")] List<ImageGesture> gestureList)
    {

        //Arrange
        ImageGestureRecognizer imageGestureRecognizer = gameobject.AddComponent<ImageGestureRecognizer>();
       // Stopwatch stopwatch = new Stopwatch();

        // Act
        //stopwatch.Start();
        List<RecognizeOutput> outputList = imageGestureRecognizer.RecognizeGesture(gestureList[0], new List<IGesture>(gestureList));
        //stopwatch.Stop();
        // Assert
        //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
        yield return false;
    }
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator OpenCvImageRecognition([ValueSource("ImageGesturePath")] List<ImageGesture> gestureList)
    {
        //Arrange
        OpenCvGestureRecognizer openCvGestureRecognizer = gameobject.AddComponent<OpenCvGestureRecognizer>();
        //Stopwatch stopwatch = new Stopwatch();

        // Act
        //stopwatch.Start();
        List<RecognizeOutput> outputList = openCvGestureRecognizer.RecognizeGesture(gestureList[0], new List<IGesture>(gestureList));
        //stopwatch.Stop();
        // Assert
        //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
        yield return false;
    }
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator VectorRecognition([ValueSource("VectorGesturePath")] List<VectorGesture> gestureList)
    {

        //Arrange
        VectorGestureRecognizer vectorGestureRecognizer = gameobject.AddComponent<VectorGestureRecognizer>();
        //Stopwatch stopwatch = new Stopwatch();

        // Act
        //stopwatch.Start();
        List<RecognizeOutput> outputList = vectorGestureRecognizer.RecognizeGesture(gestureList[0], new List<IGesture>(gestureList));
        //stopwatch.Stop();
        // Assert
        //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
        yield return false;
    }
    private static IEnumerable ImageGesturePath()
    {
        ImageGestureDatabase database = AssetDatabase.LoadAssetAtPath<ImageGestureDatabase>("Assets/Resources/SavedGestures/test0/test0.asset");
        database.InitGestureDatabase();
        List<ImageGesture> imageGestures = new List<ImageGesture>();
        foreach (var item in database.gestures)
        {
            imageGestures.Add((ImageGesture)item);
        }
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < database.gestures.Count; i++)
            {
                List<ImageGesture> result = new List<ImageGesture>(imageGestures);
                ImageGesture tempGesutre = result[i];
                result.RemoveAt(i);
                result.Insert(0, tempGesutre);
                yield return result;
            }
        }
        
    }
    private static IEnumerable VectorGesturePath()
    {
        VectorGestureDatabase database = AssetDatabase.LoadAssetAtPath<VectorGestureDatabase>("Assets/Resources/SavedGestures/VectorTest0/VectorTest0.asset");
        database.InitGestureDatabase();
        List<VectorGesture> vectorGesture = new List<VectorGesture>();
        foreach (var item in database.gestures)
        {
            vectorGesture.Add((VectorGesture)item);
        }
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < database.gestures.Count; i++)
            {
                List<VectorGesture> result = new List<VectorGesture>(vectorGesture);
                VectorGesture tempGesutre = result[i];
                result.RemoveAt(i);
                result.Insert(0, tempGesutre);
                yield return result;
            }
        }

    }
}
