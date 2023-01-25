using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class MagicCircleController : MonoBehaviour
{
    [SerializeField]
    Transform movingCircle;
    [SerializeField]
    Light magicLight;
    [SerializeField]
    bool playerInCircle;
    [SerializeField]
    float progress =0;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 maxPos;
    private Vector3 startPos;
    Material mat;
    Color tempColor;
    [SerializeField]
    float maxLightRadius;
    LevelController levelController;
    bool levelUpDone;
    ParticleSystem particleSystem;
    float startEmissionRate;
    PortalSpotManager portalSpotManager;
    [SerializeField] string nextSceneName;

    private void Start()
    {
        startPos=transform.position;
        mat=movingCircle.GetComponent<MeshRenderer>().material;
        levelController=Player.instance.GetComponentInChildren<LevelController>();
        particleSystem = GetComponent<ParticleSystem>();
        startEmissionRate = particleSystem.emissionRate;
        portalSpotManager= GetComponent<PortalSpotManager>();
    }
    private void Update()
    {
        if(playerInCircle&&portalSpotManager.portalIsActive)
        {
            if (progress > 0.999F) progress = 1;
            progress = Mathf.Lerp(progress, 1, speed * Time.deltaTime);
            ChangeView();
            if (progress == 1 && !levelUpDone)
            {
                levelUpDone = true;
                levelController.LevelUp();
                StartCoroutine(LoadNextLevel());
            }
        }
        else
        {
            if (progress < 0.001F) progress = 0;
            progress = Mathf.Lerp(progress, 0, speed * Time.deltaTime);
            ChangeView();
        }
    }

    private void ChangeView()
    {
        movingCircle.transform.position = startPos + progress * maxPos;
        movingCircle.transform.rotation = Quaternion.Euler(0, progress * 360, 0);
        tempColor= new Color(1, 1 - progress, 1);
        mat.color = tempColor;
        mat.SetColor("_EmissionColor", tempColor);
        magicLight.range = progress * maxLightRadius;
        particleSystem.emissionRate = startEmissionRate * progress;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))playerInCircle= true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInCircle = false;
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(nextSceneName);
        yield return null; 
    }
}
