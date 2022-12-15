using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
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
    [SerializeField]
    private float freq;
    private void Start()
    {
        text.text = name;
        background=GetComponent<Image>();
        inputSimulator = new InputSimulator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lastCollider!=null|| other.attachedRigidbody.GetComponent<HandCollider>()==null) return;
        lastCollider= other;
        background.color = Color.gray;
        inputSimulator.Keyboard.KeyDown(Key);
        //if(other.GetComponent<HandCollider>().hand.hand.handType==)
        SteamVR_Actions.default_Haptic[other.attachedRigidbody.GetComponent<HandCollider>().hand.hand.handType].Execute(0, 0.07F, freq, 0.3F);
    }
    private void OnTriggerExit(Collider other)
    {
        if (lastCollider != other|| other.attachedRigidbody.GetComponent<HandCollider>() == null) return;
        inputSimulator.Keyboard.KeyUp(Key);
        lastCollider = null;
        background.color = Color.white;
    }
}
