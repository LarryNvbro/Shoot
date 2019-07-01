using UnityEngine;
using System.Collections;

/// <summary>
/// Vector3 extension class
/// </summary>
public static class Vector3Extension
{
    public static void SetX(this Vector3 v, float newX)
    {
        v.x = newX;
    }

    public static void SetY(this Vector3 v, float newY)
    {
        v.y = newY;
    }

    public static void SetZ(this Vector3 v, float newZ)
    {
        v.z = newZ;
    }
}
