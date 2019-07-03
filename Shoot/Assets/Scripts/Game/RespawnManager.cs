using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform playerTr;

    public GameObject[] enemyPrefabs;
    public Transform respawnTr;
    public float respawnDelay = 0.7f;
    public float waveDelay = 10.0f;
    public float offset = 1.0f;

    public void ActiveRespawn(bool isRespawn)
    {
        if (isRespawn)
            StartCoroutine("RespawnEnemy");
        else
            StopCoroutine("RespawnEnemy");
    }

    private IEnumerator RespawnEnemy()
    {
        int wave = 0;
        while (true)
        {
            ++wave;
            GameManager.Instance.SetWave(wave);
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(respawnDelay);
                float range = (float)Screen.width / (float)Screen.height * Camera.main.orthographicSize;
                int enemyCount = Random.Range(1, 4);
                //for (int i = 0; i < enemyCount; i++)
                {
                    int prefabIndex = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemy = Instantiate(enemyPrefabs[prefabIndex], respawnTr.position + new Vector3(Random.Range(-range + offset, range - offset), 0, 0), Quaternion.identity);
                    //EnemyControl ctrl = enemy.GetComponent<EnemyControl>();
                    //var sizeX = enemy.GetComponent<BoxCollider2D>().size.x / 2;
                    //Vector3 pos = new Vector3(playerTr.position.x + i * sizeX, playerTr.position.y);
                    //enemy.SetDestination(pos);
                }
            }
            yield return new WaitForSeconds(waveDelay);
        }
    }
}
