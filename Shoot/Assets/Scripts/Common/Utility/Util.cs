using UnityEngine;
using System.Collections;
using System;

public class Util
{
    public static bool CheckEnumValue<T>(int value)
    {
        if (typeof(T).BaseType != typeof(Enum))
        {
            throw new ArgumentException("T must be of type System.Enum");
        }

        Array arr = Enum.GetValues(typeof(T));
        bool result = false;
        foreach (T item in arr)
        {
            if (value == Convert.ToInt32(item))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public static IEnumerator WaitForAnimationEnd(GameObject go, string stateName = null)
    {
        Animator animator = go.GetComponent<Animator>();

        if (animator != null)
        {
            animator.Rebind();
            if (stateName != null)
                animator.Play(stateName);

            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 애니메이션 끝날때까지 대기
            yield return new WaitForSeconds(animatorStateInfo.length);
        }
    }

    public static Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    public static Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    public static bool CheckLineIntersect(Vector2 line1point1, Vector2 line1point2, Vector2 line2point1, Vector2 line2point2)
    {
        Vector2 a = line1point2 - line1point1;
        Vector2 b = line2point1 - line2point2;
        Vector2 c = line1point1 - line2point1;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float denominator = a.y * b.x - a.x * b.y;

        if (denominator == 0)
        {
            return false;
        }
        else if (denominator > 0)
        {
            if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 || betaNumerator > denominator)
            {
                return false;
            }
        }
        else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 || betaNumerator < denominator)
        {
            return false;
        }
        return true;
    }

    public static bool CollisionLineCircle(Vector2 line1, Vector2 line2, Vector2 center, float r)
    {
        bool inside1 = CollisionPointCircle(line1, center, r);
        bool inside2 = CollisionPointCircle(line2, center, r);
        if (inside1 || inside2) return true;

        float lineLen = Vector2.Distance(line1, line2);
        float dot = (((center.x - line1.x) * (line2.x - line1.x)) + ((center.y - line1.y) * (line2.y - line1.y))) / Mathf.Pow(lineLen, 2);

        float closestX = line1.x + (dot * (line2.x - line1.x));
        float closestY = line1.y + (dot * (line2.y - line1.y));
        Vector2 closest = new Vector2(closestX, closestY);

        bool onSegment = CollisionLinePoint(line1, line2, closest);
        if (!onSegment) return false;

        if (Vector2.Distance(closest, center) <= r)
            return true;

        return false;
    }

    public static bool CollisionPointCircle(Vector2 pt, Vector2 center, float r)
    {
        if (Vector2.Distance(pt, center) <= r)
            return true;
        return false;
    }

    public static bool CollisionLinePoint(Vector2 line1, Vector2 line2, Vector2 pt)
    {
        float d1 = Vector2.Distance(pt, line1);
        float d2 = Vector2.Distance(pt, line2);

        float lineLen = Vector2.Distance(line1, line2);
        float buffer = 0.1f;

        if (d1 + d2 >= lineLen - buffer && d1 + d2 <= lineLen + buffer)
            return true;
        return false;
    }

    //public static string GetSortingLayerName(int num)
    //{
    //    string name = "Default";
    //    switch (num)
    //    {
    //        case 0:
    //            name = "Background";
    //            break;
    //        case 1:
    //            name = "Tile";
    //            break;
    //        case 2:
    //            name = "UpTile";
    //            break;
    //        case 3:
    //            name = "TileBlock";
    //            break;
    //        case 4:
    //            name = "UnderBlock";
    //            break;
    //        case 5:
    //            name = "Block";
    //            break;
    //        case 6:
    //            name = "PickBlock";
    //            break;
    //        case 7:
    //            name = "BlockObj";
    //            break;
    //        case 8:
    //            name = "Obj";
    //            break;
    //        case 9:
    //            name = "UpObj";
    //            break;
    //    }
    //    return name;
    //}
}
