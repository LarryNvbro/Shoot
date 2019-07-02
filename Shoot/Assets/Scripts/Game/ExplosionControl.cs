using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour
{
    public AudioClip soundFx;
    public AudioSource audioSource;

    public float maxPitch = 0.6f;
    public float minPitch = 0.2f;

    void Start()
    {
        float pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(soundFx, pitch);
        Invoke("Destroy", 0.75f);

    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
