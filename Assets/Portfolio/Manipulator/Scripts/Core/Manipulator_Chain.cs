using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class Manipulator_Chain
{
    public List<Manipulator> Manipulators = new List<Manipulator>();

    public void Add(Manipulator manipulator)
    {
        Manipulators.Add(manipulator);
    }

    public void Remove(Manipulator manipulator)
    {
        Manipulators.Remove(manipulator);
    }

    public void Run()
    {
        for (int i = 0; i < Manipulators.Count; i++)
        {
            Manipulators[i].Run();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < Manipulators.Count; i++)
        {
            Manipulators[i].Reset();
        }
    }

    public JSONNode Save()
    {
        JSONNode node = JSONNode.Empty;
        JSONArray array = new JSONArray();
        for (int i = 0; i < Manipulators.Count; i++)
        {
            array.Add(Manipulators[i].SaveJson());
        }
        node["Chains"] = array;
        return node;
    }

    public void Load(Manipulator manipulator, JSONNode node)
    {
        var chains = node["Chains"].AsArray;
        for (int i = 0; i < chains.Count; i++)
        {
            Manipulators.Add(Manipulator.LoadJson(chains[i]));
        }
    }

    public void ToMono(Manipulator_Mono manipulator_Mono)
    {
        for (int i = 0; i < Manipulators.Count; i++)
        {
            GameObject ob = new GameObject("Chain " + i);
            ob.transform.parent = manipulator_Mono.transform;
            var manipulator_Mono_Chain = ob.AddComponent<Manipulator_Mono>();
            Manipulators[i].ToMono(manipulator_Mono_Chain);
            manipulator_Mono.gameObject.AddComponent<Manipulator_Mono_Chain>().manipulator_Mono = manipulator_Mono_Chain;
        }
    }

    public void SetTransform(Transform transform, bool force)
    {
        for (int i = 0 ; i < Manipulators.Count; i++)
        {
            Manipulators[i].SetTransform(transform, force);
        }
    }
}