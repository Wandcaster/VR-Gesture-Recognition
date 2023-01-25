using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelModel : MonoBehaviour
{
    public int currentLevel = 1;
    public List<Behaviour> spells;
    public GameObject LevelPopup;
    public UnityEvent OnLevelUp;
    

}
