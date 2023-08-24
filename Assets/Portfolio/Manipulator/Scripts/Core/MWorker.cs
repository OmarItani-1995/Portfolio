using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;


[System.Serializable]
public abstract class MWorker
{
#if UNITY_EDITOR
    public bool Foldout = false;
#endif

    [HideInInspector] private Manipulator manipulator;

    public Manipulator Manipulator
    {
        get
        {
            return manipulator;
        }
    }

    public Transform Transform
    {
        get
        {
            return manipulator.Transform;
        }
    }

    public Renderer Renderer 
    {
        get 
        {
            return manipulator.Renderer;
        }
    }

    public float ElapsedTime
    {
        get
        {
            return manipulator.ElapsedTime;
        }
    }

    public float RemapedTime
    {
        get
        {
            return manipulator.RemapedTime;
        }
    }

    public MWorker(Manipulator manipulator)
    {
        this.manipulator = manipulator;
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Stop()
    {

    }

    public virtual void Reset()
    {

    }

    public abstract void OnGui(Manipulator manipualtor);

    public JSONNode Save()
    {
        JSONNode node = JSONNode.Empty;
        node["Type"] = GetType().Name;
        OnSave(node);
        return node;
    }

    public abstract void OnSave(JSONNode node);

    public static MWorker Load(Manipulator manipulator, JSONNode node)
    {
        var type = Type.GetType(node["Type"].Value);
        var worker = Activator.CreateInstance(type, manipulator) as MWorker;
        worker.OnLoad(node);
        return worker;
    }

    public abstract void OnLoad(JSONNode node);

    public abstract void ToMono(Manipulator_Mono manipulator);
}