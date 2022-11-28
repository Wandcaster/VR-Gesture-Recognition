using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using WindowsInput;

public class KeyController : VRUI
{
    [SerializeField]
    TextMeshProUGUI text;
    Collider lastCollider;
    private Image background;
    InputSimulator inputSimulator;
    [SerializeField]
    WindowsInput.Native.VirtualKeyCode Key;
    private void Start()
    {
        text.text = name;
        background=GetComponent<Image>();
        inputSimulator = new InputSimulator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lastCollider!=null) return;
        lastCollider= other;
        background.color = Color.gray;
        inputSimulator.Keyboard.KeyDown(Key);
        SteamVR_Actions.default_Haptic[SteamVR_Input_Sources.LeftHand].Execute(0.5F, 0.5F,320,1);
    }
    private void OnTriggerExit(Collider other)
    {
        if (lastCollider != other) return;
        inputSimulator.Keyboard.KeyUp(Key);
        lastCollider = null;
        background.color = Color.white;
    }
}
