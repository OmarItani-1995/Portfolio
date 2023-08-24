using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem _instance = null;

    private static EventSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("EventSystem").AddComponent<EventSystem>();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    private Dictionary<System.Type, List<Type>> _events = new Dictionary<System.Type, List<Type>>();

    public void Initialize()
    {
        var eventListeners = Assembly
                                .GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => !t.IsAbstract && (typeof(Event_Listener<>).IsAssignableFrom(t) || GetInheritanceChain(t).Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(Event_Listener<>))));

        foreach (var listener in eventListeners)
        {
            var eventType = listener.BaseType.GetGenericArguments()[0];
            Debug.Log(eventType);
            if (!_events.ContainsKey(eventType))
            {
                _events.Add(eventType, new List<Type>());
            }
            _events[eventType].Add(listener);
        }
    }

    public static Type[] GetInheritanceChain(Type type)
    {
        var inheritanceChain = new List<Type>();

        var current = type;
        while (current.BaseType != null)
        {
            inheritanceChain.Add(current.BaseType);
            current = current.BaseType;
        }

        return inheritanceChain.ToArray();
    }

    public static void FireEvent<T>(T ev)
    {
        var type = typeof(T);
        if (!Instance._events.ContainsKey(type))
        {
            Debug.Log($"No listeners for event {type}");
            return;
        }

        Instance._events[type].Select(x => Activator.CreateInstance(x) as Event_Listener<T>).ToList().ForEach(x => x.OnEvent(ev));
    }
}

public abstract class Event_Listener<T>
{
    public abstract void OnEvent(T ev);
}
