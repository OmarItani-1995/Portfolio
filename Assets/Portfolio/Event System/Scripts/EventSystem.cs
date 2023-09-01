using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem _instance = null;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Initialize();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private Dictionary<System.Type, List<Event_Listener>> listeners = new();

    public void Initialize()
    {
        IEnumerable<Type> eventListeners = GetListenerTypes();
        foreach (var listener in eventListeners)
        {
            if (listener.GetInterface(nameof(IDisabledListener)) != null) continue;
            var eventType = listener.BaseType.GetGenericArguments()[0];
            if (!listeners.ContainsKey(eventType))
            {
                listeners.Add(eventType, new List<Event_Listener>());
            }
            listeners[eventType].
                Add((Event_Listener)Activator.CreateInstance(listener));
        }
    }

    private static IEnumerable<Type> GetListenerTypes()
    {
        return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(
                    t => !t.IsAbstract
                    && (typeof(Event_Listener<>).IsAssignableFrom(t)
                    || GetInheritanceChain(t)
                        .Any(
                            x =>
                                x.IsGenericType
                                && x.GetGenericTypeDefinition() == typeof(Event_Listener<>))));
    }

    public static List<Type> GetInheritanceChain(Type type)
    {
        var inheritanceChain = new List<Type>();

        var current = type;
        while (current.BaseType != null)
        {
            inheritanceChain.Add(current.BaseType);
            current = current.BaseType;
        }

        return inheritanceChain;
    }

    public static void FireEvent<T>(T ev) where T : Event
    {
        var type = typeof(T);
        if (!_instance.listeners.ContainsKey(type))
        {
            Debug.Log($"No listeners for event {type}");
            return;
        }

        var listeners = _instance.listeners[type];
        foreach (var listener in listeners)
        {
            (listener as Event_Listener<T>).OnEvent(ev);
        }
    }
}
public class Event
{

}

public class Event_Listener
{

}

public abstract class Event_Listener<T> : Event_Listener where T : Event
{
    public abstract void OnEvent(T ev);
}

public interface IDisabledListener
{

}