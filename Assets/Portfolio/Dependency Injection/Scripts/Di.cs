using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;

#nullable enable

public class Di : MonoBehaviour
{
    private static Di _instance = null;
    private static Di Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Di").AddComponent<Di>();
            }
            return _instance;
        }
    }

    private Dictionary<Type, Class_Description> _descriptors = new Dictionary<Type, Class_Description>();


    public static void AddSingleton<T>(bool forceRecreate = false) where T : class
    {
        AddSingleton<T>(null, () => Activator.CreateInstance<T>(), forceRecreate);
    }

    public static void AddSingleton<T>(T? createdObject, bool forceRecreate = false) where T : class
    {
        AddSingleton<T>(createdObject, null, forceRecreate);
    }

    public static void AddSingleton<T>(Func<T>? creator, bool forceRecreate = false) where T : class
    {
        AddSingleton<T>(null, creator);
    }

    public static void AddSingleton<T>(T? createdObject, Func<T>? creator, bool forceRecreate = false) where T : class
    {
        var type = typeof(T);
        if (Instance._descriptors.ContainsKey(type))
        {
            Debug.Log($"Service of Type {type} has already been registered");
            return;
        }
        Class_Description<T> descriptor = new Class_Description<T>(forceRecreate, createdObject, creator);
        Instance._descriptors.Add(type, descriptor);
    }

    public static void OverrideSingleton<T>(T? newSingleton) where T : class
    {
        var type = typeof(T);
        if (!Instance._descriptors.ContainsKey(type))
        {
            Debug.Log($"Service of Type {type} hasn't been registered");
            return;
        }
        Instance._descriptors.Remove(type);
        AddSingleton<T>(newSingleton);
    }

    public static T? Get<T>() where T : class
    {
        var type = typeof(T);
        if (!Instance._descriptors.ContainsKey(type))
        {
            Debug.LogError($"Service of Type {type} hasn't been registered");
            return null;
        }

        var descriptor = Instance._descriptors[type];
        return descriptor.GetInstance() as T;
    }

    public static void AddSingleton<TType, TImplementation>(bool forceRecreate = false) where TType : class where TImplementation : class, TType
    {
        AddSingleton<TType, TImplementation>(null, () => Activator.CreateInstance<TImplementation>(), forceRecreate);
    }

    public static void AddSingleton<TType, TImplementation>(TImplementation? createdObject, bool forceRecreate = false) where TType : class where TImplementation : class, TType
    {
        AddSingleton<TType, TImplementation>(createdObject, null, forceRecreate);
    }

    public static void AddSingleton<TType, TImplementation>(Func<TImplementation>? creator, bool forceRecreate = false) where TType : class where TImplementation : class, TType
    {
        AddSingleton<TType, TImplementation>(null, creator, forceRecreate);
    }

    public static void AddSingleton<TType, TImplementation>(TImplementation? createdObject, Func<TImplementation>? creator, bool forceRecreate = false) where TType : class where TImplementation : class, TType
    {
        var type = typeof(TType);
        if (Instance._descriptors.ContainsKey(type))
        {
            Debug.Log($"Service of Type {type} has already been registered");
            return;
        }
        Class_Description<TImplementation> descriptor = new Class_Description<TImplementation>(forceRecreate, createdObject, creator);
        Instance._descriptors.Add(type, descriptor);
    }

    public static void AddGroup<T>(bool forceRecreate = false) where T : class
    {
        var type = typeof(T);
        if (Instance._descriptors.ContainsKey(type))
        {
            Debug.Log($"Group of Type {type} has already been registered");
            return;
        }

        List<Type> types = new List<Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var t in assembly.GetTypes())
            {
                if (!t.IsAbstract && !t.IsInterface && t.IsSubclassOf(type))
                {
                    types.Add(t);
                }
            }
        }

        Class_Description<List<T>> descriptor = new Class_Description<List<T>>(
            forceRecreate: forceRecreate,
            createdObject: null,
            creator: () =>
            {
                return types.Select(x => Activator.CreateInstance(x) as T).Cast<T>().ToList();
            });
        Instance._descriptors.Add(type, descriptor);
    }

    public static List<T>? GetGroup<T>() where T : class
    {
        var type = typeof(T);
        if (!Instance._descriptors.ContainsKey(type))
        {
            Debug.LogError($"Service of Type {type} hasn't been registered");
            return null;
        }

        var descriptor = Instance._descriptors[type];
        return descriptor.GetInstance() as List<T>;
    }


}

public abstract class Class_Description
{

    public bool ForceRecreate { get; set; } = false;
    protected Class_Description(bool forceRecreate)
    {
        ForceRecreate = forceRecreate;
    }

    public abstract object? GetInstance();
}

public class Class_Description<T> : Class_Description where T : class
{
    public T? createdObject;
    public Func<T>? creator;

    public Class_Description(bool forceRecreate, T? createdObject, Func<T>? creator) : base(forceRecreate)
    {
        this.createdObject = createdObject;
        this.creator = creator;
    }

    public override object? GetInstance()
    {
        return ForceRecreate ? creator?.Invoke() : createdObject ??= creator?.Invoke();
    }
}