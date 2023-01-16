using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateOfChangePosition : MonoBehaviour
{
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private float deltaTime;
    private WandModel wandModel;

    // Start is called before the first frame update
    void Start()
    {
        wandModel=GetComponent<WandModel>();
        currentPosition = transform.position;
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime;
        previousPosition = currentPosition;
        currentPosition = transform.position;
        Vector3 rateOfChange = (currentPosition - previousPosition) / deltaTime;
        wandModel.rateOfChangePosition = rateOfChange;
    }
}
