using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class Manipulator_Workers
{
    public List<MWorker> Workers = new List<MWorker>();

    public void Add(MWorker worker)
    {
        Workers.Add(worker);
    }

    public void Remove(MWorker worker)
    {
        Workers.Remove(worker);
    }

    public void Start()
    {
        for (int i = 0; i < Workers.Count; i++)
        {
            Workers[i].Start();
        }
    }

    public void Update()
    {
        for (int i = 0; i < Workers.Count; i++)
        {
            Workers[i].Update();
        }
    }

    public void Stop()
    {
        for (int i = 0; i < Workers.Count; i++)
        {
            Workers[i].Stop();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < Workers.Count; i++)
        {
            Workers[i].Reset();
        }
    }

    public JSONNode Save()
    {
        JSONNode node = JSONNode.Empty;
        JSONArray array = new JSONArray();
        for (int i = 0; i < Workers.Count; i++)
        {
            array.Add(Workers[i].Save());
        }
        node["Workers"] = array;
        return node;
    }

    public void Load(Manipulator manipulator, JSONNode jSONNode)
    {
        var array = jSONNode["Workers"].AsArray;
        for (int i = 0; i < array.Count; i++)
        {
            var worker = MWorker.Load(manipulator, array[i]);
            Workers.Add(worker);
        }
    }

    public void ToMono(Manipulator_Mono manipulator_Mono)
    {
        for (int i = 0 ; i < Workers.Count; i++)
        {
            Workers[i].ToMono(manipulator_Mono);
        }
    }
}