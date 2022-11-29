using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using VRGesureRecognition;

public class RecognitionTest
{
    ImageGestureRecognizer imageGestureRecognizer;
    [SetUp]
    public void InitData()
    {
        GameObject gameobject = new GameObject();
        imageGestureRecognizer = gameobject.AddComponent<ImageGestureRecognizer>();
    }
    // A test with the [RequiresPlayMode] tag ensures that the test is always run inside PlayMode.
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator Recognition([ValueSource("AdditionCases")] List<string> pathList)
    {
        
        //Arrange
        List<IGesture> imageGestures = new List<IGesture>();
        foreach (var path in pathList)
        {
            imageGestures.Add(new ImageGesture(AssetDatabase.LoadAssetAtPath<ImageGestureData>(path)));
        }
       
        // Act
        List<RecognizeOutput> outputList = imageGestureRecognizer.RecognizeGesture((ImageGesture)imageGestures[0], imageGestures);

        // Assert
        Debug.Log("MainGesture:" + outputList[0].recognizedGesture.gestureName);
        for (int i = 1; i < outputList.Count; i++)
        {
            Debug.Log(outputList[i].recognizedGesture.gestureName + ": " + outputList[i].probability);
            Assert.Less(outputList[i].probability, 0.6f);
        }
        yield return false;
    }
    private static IEnumerable AdditionCases()
    {
        
        List<string> path = new List<string>
        {
            "Assets/Resources/SavedGestures/test0/S/S.asset",
            "Assets/Resources/SavedGestures/test0/W/W.asset",
            "Assets/Resources/SavedGestures/test0/X/X.asset",
            "Assets/Resources/SavedGestures/test0/black/X.asset"
        };
        
        for (int i = 0; i < path.Count; i++)
        {
            List<string> result = new List<string>(path);
            string tempString = result[i];
            result.RemoveAt(i);
            result.Insert(0, tempString);
            yield return result;
        }
    }
}
