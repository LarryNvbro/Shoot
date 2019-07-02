using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject obj;
    public Transform respawnTr;
    public float respawnDelay = 0.7f;
    public float offset = 1.0f;

    void Start()
    {
        StartCoroutine(RespawnEnemy());
    }

    IEnumerator RespawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnDelay);
            float range = (float)Screen.width / (float)Screen.height * Camera.main.orthographicSize ;
            Instantiate(obj, respawnTr.position + new Vector3(Random.Range(-range + offset, range - offset), 0, 0), Quaternion.identity);

        }
    }
}
