using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;
using static SceneLoadActions;

public class SceneLoadActions : MonoBehaviour
{
    [Serializable]
    public struct LoadSceneData
    {
        public string sceneName;
        public Vector3 targetPosition;
    }

    [SerializeField] private Transform player;
    [SerializeField] List<LoadSceneData> loadSceneData;
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
        Debug.Log(scene.name);
        LoadSceneData data = loadSceneData.First(x => x.sceneName == scene.name);
        player.transform.position = data.targetPosition;
    }
}
