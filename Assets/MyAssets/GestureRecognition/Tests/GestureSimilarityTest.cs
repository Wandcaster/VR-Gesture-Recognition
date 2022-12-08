using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenCvSharp;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using VRGesureRecognition;

public class GestureSimilarityTest
{
    GameObject gameobject;
    [SetUp]
    public void InitData()
    {
        gameobject = new GameObject();
    }
    // A test with the [RequiresPlayMode] tag ensures that the test is always run inside PlayMode.
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator ImageRecognition([ValueSource("GesturePath")] List<ImageGesture> gestureList)
    {

        //Arrange
        ImageGestureRecognizer imageGestureRecognizer = gameobject.AddComponent<ImageGestureRecognizer>();


        // Act
        List<RecognizeOutput> outputList = imageGestureRecognizer.RecognizeGesture(gestureList[0], new List<IGesture>(gestureList));

        // Assert
        Debug.Log("MainGesture:" + outputList[0].recognizedGesture.gestureName);
        for (int i = 0; i < outputList.Count; i++)
        {
            Debug.Log(outputList[i].recognizedGesture.gestureName + ": " + outputList[i].probability);
            if (i != 0) Assert.Less(outputList[i].probability, 0.6f);
        }
        yield return false;
    }
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator OpenCvImageRecognition([ValueSource("GesturePath")] List<ImageGesture> gestureList)
    {

        //Arrange
        OpenCvGestureRecognizer openCvGestureRecognizer = gameobject.AddComponent<OpenCvGestureRecognizer>();

        // Act
        List<RecognizeOutput> outputList = openCvGestureRecognizer.RecognizeGesture(gestureList[0], new List<IGesture>(gestureList));

        // Assert
        Debug.Log("MainGesture:" + outputList[0].recognizedGesture.gestureName);
        for (int i = 1; i < outputList.Count; i++)
        {
            Debug.Log(outputList[i].recognizedGesture.gestureName + ": " + outputList[i].probability);
           Assert.Less(outputList[i].probability, 0);
        }
        yield return false;
    }
    private static IEnumerable GesturePath()
    {
        ImageGestureDatabase database= AssetDatabase.LoadAssetAtPath<ImageGestureDatabase>("Assets/Resources/SavedGestures/test0/test0.asset");
        database.InitGestureDatabase();
        List<ImageGesture> imageGestures = new List<ImageGesture>();
        foreach (var item in database.gestures)
        {
            imageGestures.Add((ImageGesture)item);
        }
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
