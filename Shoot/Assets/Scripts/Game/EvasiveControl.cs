using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SHBoundary
{
    public float xMin, xMax, yMin, yMax;
}

public class EvasiveControl : MonoBehaviour
{
    public SHBoundary boundary;
    public float tilt;
    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    private float currentSpeed = -3;
    private float targetManeuver;

    void Start()
    {
        //currentSpeed = GetComponent<Rigidbody2D>().velocity.y;
        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (true)
        {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(GetComponent<Rigidbody2D>().velocity.x, targetManeuver, smoothing * Time.deltaTime);

        GetComponent<Rigidbody2D>().velocity = new Vector2(newManeuver, currentSpeed);
        GetComponent<Rigidbody2D>().position = new Vector2
        (
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
        );

        GetComponent<Rigidbody2D>().rotation = GetComponent<Rigidbody2D>().velocity.x * -tilt;
    }
}
