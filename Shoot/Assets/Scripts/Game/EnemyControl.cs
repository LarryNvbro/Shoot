using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public int hp = 6;
    public int initHp = 6;

    public Transform tr;
    public GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            --hp;
            if(hp == 0)
            {
                //score++
                GameManager.Instance.AddScore(100);
                //effect play
                Instantiate(effect, tr.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
        else if( collision.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }
    }
}
