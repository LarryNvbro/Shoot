using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// Sets localPosition to Vector3.zero, localRotation to Quaternion.identity, localScale to Vector3.one
    /// </summary>
    public static void Reset(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static void SetPositionX(this Transform t, float newX)
    {
        t.position = new Vector3(newX, t.position.y, t.position.z);
    }

    public static void SetPositionY(this Transform t, float newY)
    {
        t.position = new Vector3(t.position.x, newY, t.position.z);
    }

    public static void SetPositionZ(this Transform t, float newZ)
    {
        t.position = new Vector3(t.position.x, t.position.y, newZ);
    }

    public static void SetPosition(this Transform t, float newX, float newY, float newZ)
    {
        t.position = new Vector3(newX, newY, newZ);
    }

    public static void SetLocalPositionX(this Transform t, float newX)
    {
        t.localPosition = new Vector3(newX, t.localPosition.y, t.localPosition.z);
    }

    public static void SetLocalPositionY(this Transform t, float newY)
    {
        t.localPosition = new Vector3(t.localPosition.x, newY, t.localPosition.z);
    }

    public static void SetLocalPositionZ(this Transform t, float newZ)
    {
        t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZ);
    }

    public static void SetLocalPosition(this Transform t, float newX, float newY, float newZ)
    {
        t.localPosition = new Vector3(newX, newY, newZ);
    }

    public static void SetLocalScale(this Transform t, float newX, float newY, float newZ)
    {
        t.localScale = new Vector3(newX, newY, newZ);
    }

    public static bool HasChild(this Transform t, string name)
    {
        return (t.Find(name) != null);
    }
}
