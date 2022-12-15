using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Valve.VR;
using VRGesureRecognition;

public class TestWithAnimator
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Test");
    }
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator GetureRecognitionTest()
    {
        GestureManager gestureManager = GameObject.Find("GestureManager").GetComponent<GestureManager>();
        gestureManager.isEnabled= true;
        gestureManager.StartTrialRenderer();
        Animation animation =gestureManager.trackedPoint.GetComponent<Animation>();
        animation.Play();
        yield return null;
        while (animation.isPlaying)
        {
            yield return null;
        }
        gestureManager.Recognize();
        yield return null;
    }
}
