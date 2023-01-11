using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class SceneLoadActions : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 targetPosition; 
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SceneManager.sceneLoaded += SetPlayerPos;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetPlayerPos;

    }

    private void SetPlayerPos(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = targetPosition;
    }
}
