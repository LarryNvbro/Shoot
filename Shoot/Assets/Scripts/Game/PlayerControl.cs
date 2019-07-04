using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int hp = 10;
    public int initHp = 10;

    public Rigidbody2D rb;
    public float speed = 300.0f;

    public Transform tr;
    public float offset = 1.0f;

    private float h; // horizontal
    private float v; // vertical

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        hp = initHp;
    }
    
    public void SetPos(float diffX)
    {
        Vector3 pos = transform.position;
        pos.x += diffX;
        transform.position = pos;
        FixedPos();
    }

    public void ActiveFire(bool isFire)
    {
        GetComponent<FireControl>().ActiveFire(isFire);
    }

    private void FixedPos()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");

        //Vector2 dir = new Vector2(h, v);
        //rb.velocity = dir * speed * Time.deltaTime;

        float size = Camera.main.orthographicSize;
        if (tr.position.y >= size - offset)
        {
            tr.position = new Vector3(tr.position.x, size - offset);
        }
        else if (tr.position.y <= -size + offset)
        {
            tr.position = new Vector3(tr.position.x, -size + offset);
        }

        float screenRation = (float)Screen.width / (float)Screen.height;
        float wSize = Camera.main.orthographicSize * screenRation;

        if (tr.position.x >= wSize - offset)
        {
            tr.position = new Vector3(wSize - offset, tr.position.y);
        }
        else if (tr.position.x <= -wSize + offset)
        {
            tr.position = new Vector3(-wSize + offset, tr.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            --hp;
            if(hp <= 0)
            {
                Debug.Log("PlayerDie");
            }
        }
    }
}
