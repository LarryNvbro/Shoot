using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : ItemControl
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(100);
        }
        base.OnTriggerEnter2D(collision);
    }
}
