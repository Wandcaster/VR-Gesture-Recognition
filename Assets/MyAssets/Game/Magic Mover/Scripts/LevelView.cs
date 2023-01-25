using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelView : MonoBehaviour
{
    private LevelModel levelModel;

    private void Start()
    {
        levelModel= GetComponent<LevelModel>();
        levelModel.OnLevelUp.AddListener(DisplayLevelUp);
    }
    public void DisplayLevelUp()
    {
        Debug.Log("I level up");
    }
}
