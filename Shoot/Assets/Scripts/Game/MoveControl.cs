using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    public Transform tr;
    public float speed;

    void Update()
    {
        tr.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
