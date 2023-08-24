using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Poolable_Behavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static GameObject Prefab;
    public static string LoadLocation
    {
        get
        {
            return typeof(T).Name;
        }
    }

    protected static List<T> pool;
    protected static List<T> active;
    public static int ActiveCount
    {
        get
        {
            return active == null ? 0 : active.Count;
        }
    }

    public static T Get(Transform parent = null)
    {
        if (pool == null) pool = new List<T>();
        if (pool.Count > 0)
        {
            return pool.GetLastAndRemove();
        }
        else
        {
            if (Prefab == null)
            {
                Prefab = Resources.Load<GameObject>(LoadLocation);
            }
            var prefab = Instantiate(Prefab, parent).GetComponent<T>();
            if (active == null) active = new List<T>();
            active.Add(prefab);
            return prefab;
        }
    }

    public static void Return(T obj)
    {
        active.Remove(obj);
        pool.Add(obj);
    }

    public static void Reset()
    {
        active = new List<T>();
        pool = new List<T>();
    }
}
