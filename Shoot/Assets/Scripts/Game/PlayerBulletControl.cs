using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletControl : BulletControl
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        base.OnTriggerEnter2D(collision);
    }
}
