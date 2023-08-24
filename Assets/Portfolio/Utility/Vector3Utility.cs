using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public static class Vector3Utility
{
    public static Vector3 Multiply(this Vector3 first, Vector3 second) => new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);

    public static void RemoveClosestMatch(this List<Vector3> points, List<Vector3> closest)
    {
        for (int i = 0; i < closest.Count; i++)
        {
            var point = closest[i];
            var closestIndex = point.GetClosestIndex(points);
            if (closestIndex != -1)
            {
                points.RemoveAt(closestIndex);
            }
        }
    }

    public static int GetClosestIndex(this Vector3 point, List<Vector3> points)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < points.Count; i++)
        {
            var distance = Vector3.Distance(point, points[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }
}
