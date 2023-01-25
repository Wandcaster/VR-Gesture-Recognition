using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    LevelModel levelModel;
    private void Start()
    {
        levelModel = GetComponent<LevelModel>();
        UnlockSpells();
        foreach (var item in levelModel.spells)
        {
            Debug.Log(item.enabled);
        }
    }
    public void LevelUp()
    {
        levelModel.currentLevel++;
        UnlockSpells();
        levelModel.OnLevelUp.Invoke();
        Instantiate(levelModel.LevelPopup).GetComponent<LevelUpNotificationController>().level.text=levelModel.currentLevel.ToString();
    }
    public void UnlockSpells()
    {
        foreach (var spell in levelModel.spells)
        {
            spell.enabled = false;
        }
        for (int i = 0; i < levelModel.currentLevel; i++)
        {
            levelModel.spells[i].enabled = true;
        }
    }
}
