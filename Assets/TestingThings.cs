using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class TestingThings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Player.instance.bodyDirectionGuess);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, Player.instance.bodyDirectionGuess);
    }
}
