using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    public int hp = 6;
    public int initHp = 6;
    public float speed = 1;

    public Transform tr;
    public GameObject effect;
    public GameObject coin;
    public Slider hpGauge;

    public void SetDestination(Vector3 pos)
    {
        //StartCoroutine(Move(pos));
    }

    private IEnumerator Move(Vector3 pos)
    {
        var curPos = tr.position;
        while (true)
        {
            //Vector3 dest = pos * speed * Time.deltaTime;
            //tr.position = dest;
            tr.Translate((pos - curPos) * speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            --hp;
            hpGauge.value = (float)hp / initHp;
            if(hp == 0)
            {
                //effect play
                Instantiate(effect, tr.position, Quaternion.identity);
                Instantiate(coin, tr.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
        else if( collision.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }
    }
}
