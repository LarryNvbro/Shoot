using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public float delayTime = 0.2f;
    public GameObject bullet;

    public GameObject[] pos;

    public AudioClip soundFx;
    public AudioSource audioSource;
    public float soundPitch = 0.3f;

    void Start()
    {
        //InvokeRepeating("Fire", 0.5f, delayTime);
        ActiveFire(true);
    }

    public void ActiveFire(bool isFire)
    {
        if (isFire)
            StartCoroutine("FireRealTime");
        else
            StopCoroutine("FireRealTime");
    }

    private IEnumerator FireRealTime()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            audioSource.PlayOneShot(soundFx, soundPitch);

            for (int i = 0; i < pos.Length; i++)
            {
                Instantiate(bullet, pos[i].transform.position, pos[i].transform.rotation);
            }
            yield return new WaitForSeconds(delayTime);
        }
    }

    private void Fire()
    {

        audioSource.PlayOneShot(soundFx, soundPitch);

        for (int i = 0; i < pos.Length; i++)
        {
            Instantiate(bullet, pos[i].transform.position, pos[i].transform.rotation);
        }
    }
}
