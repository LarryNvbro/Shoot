using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyControl : MonoBehaviour
{
    public int hp = 6;
    public int initHp = 6;
    public float speed = 1;

    public Transform tr;

    public GameObject damage;
    public GameObject effect;
    public GameObject coin;

    public Slider hpGauge;

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

    private void ShowDamageFx(int dmg)
    {
        GameObject go = Instantiate(damage, tr.position, Quaternion.identity, transform.Find("Canvas"));
        go.GetComponent<RectTransform>().position = new Vector3(0,0,-1);
        var tm = go.GetComponent<TextMesh>();

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(DOTween.ToAlpha(() => tm.color, color => tm.color = color, 0.0f, 0.3f))
            .AppendCallback(()=>
            {
                Destroy(go);
            });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet") ||
            collision.CompareTag("Player"))
        {
            --hp;
            //ShowDamageFx(1);
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
