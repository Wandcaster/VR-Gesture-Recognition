using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WaterBall : MonoBehaviour
{
    [SerializeField] ParticleSystem _WaterBallParticleSystem;
    [SerializeField] ParticleSystem _SplashPrefab;
    [SerializeField] ParticleSystem _SpillPrefab;
    bool firstCollision = true;
    public float hydrationValue;

    private void OnCollisionEnter(Collision collision)
    {
        if (!firstCollision)return;
        firstCollision = false;
        VisualEffect(collision);
    }
    private void VisualEffect(Collision collision)
    {
        Vector3 target = collision.contacts[0].point;
        _WaterBallParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem splas = Instantiate(_SplashPrefab, target, Quaternion.identity);
        Vector3 forward = collision.contacts[0].normal;
        forward.y = 0;
        splas.transform.forward = forward;
        //if (Vector3.Angle(startPos - target, Vector3.up) > 30)
        //{
        //    ParticleSystem spill = Instantiate(_SpillPrefab, target, Quaternion.identity);
        //    spill.transform.forward = forward;
        //}
        Destroy(gameObject, 0.5f);
    }
}
