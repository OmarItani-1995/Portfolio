using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Msg : MonoBehaviour
{
    public static Msg Instance { get; private set; }
    
    private Dictionary<Type, List<MsgData>> listeners = new Dictionary<Type, List<MsgData>>();

    void Awake()
    {
        Instance = this;
    }

    public static void Listen<T>(Action<T> message) where T : Message
    {
        var type = typeof(T);
        if (!Instance.listeners.ContainsKey(type))
        {
            Instance.listeners.Add(type, new List<MsgData>());
        }
        Instance.listeners[type].Add(new MsgData<T>(message));
    }

    public static void Queue<T>(T message) where T : Message
    {
        var type = typeof(T);
        if (Instance.listeners.ContainsKey(type))
        {
            if (Instance.listeners[type].Count == 0) return;
            for (int i = Instance.listeners.Count - 1; i >= 0; i--)
            {
                Instance.listeners[type][i].Fire(message);
            }
        }
    }

    public static void Remove<T>(Action<T> message) where T : Message
    {
        var type = typeof(T);
        if (Instance.listeners.ContainsKey(type))
        {
            Instance.listeners[type].Remove(Instance.listeners[type].Find(x => (x as MsgData<T>).action == message));
        }
    }
}

public abstract class MsgData
{
    public abstract void Fire(Message message);
}
public class MsgData<T> : MsgData where T : Message
{
    public Action<T> action;
    public MsgData(Action<T> message)
    {
        this.action = message;
    }

    public override void Fire(Message message)
    {
        action(message as T);
    }
}

public class Message
{

}