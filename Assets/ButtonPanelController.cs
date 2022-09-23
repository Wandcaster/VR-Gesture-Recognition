using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelController : MonoBehaviour
{
    [SerializeField]
    private Button addGestureMode;
    [SerializeField]
    private Button recognitionMode;

    public void ActiveButton(Button button)
    {
        if (button != addGestureMode) addGestureMode.GetComponent<Image>().color = Color.white;
        else button.GetComponent<Image>().color = Color.green;
        if (button != recognitionMode) recognitionMode.GetComponent<Image>().color = Color.white;
        else button.GetComponent<Image>().color = Color.green;
    }
    public void addGestureModeButton()
    {
        GestureManager.Instance.AddGestureMode = true;
    }
    public void recognitionModeButton()
    {
        GestureManager.Instance.AddGestureMode = false;
    }
}
