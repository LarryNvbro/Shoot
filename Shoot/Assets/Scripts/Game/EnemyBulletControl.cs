using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletControl : BulletControl
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
