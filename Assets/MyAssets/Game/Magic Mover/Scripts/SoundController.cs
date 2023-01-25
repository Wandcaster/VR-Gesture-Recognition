using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] float step;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude >= 0)
        {
            audioSource.UnPause();
            audioSource.volume= Mathf.Lerp(audioSource.volume, rb.velocity.sqrMagnitude, step);
            if (audioSource.volume < 0.00001F) audioSource.volume = 0;

        }
    }
}
