using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtility
{
    public static T GetLastAndRemove<T>(this List<T> list)
    {
        T item = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return item;
    }

    public static List<T> Copy<T>(this List<T> list)
    {
        var copy = new List<T>();
        copy.AddRange(list);
        return copy;
    }


}
