using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

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

    private void Start()
    {
        startPos=transform.position;
        mat=movingCircle.GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if(playerInCircle)
        {
            if (progress > 0.999F) progress = 1;
            progress = Mathf.Lerp(progress, 1, speed * Time.deltaTime);
            ChangeView();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))playerInCircle= true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInCircle = false;
    }

}
