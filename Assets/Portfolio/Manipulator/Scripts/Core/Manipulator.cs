using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// An object Manipulator Class, can be used to do effects on any transform/rigidbody in the scene.
/// It can also be modified to do effects on any other objects. 
/// </summary>
[System.Serializable]
public class Manipulator
{
    public Transform Transform;
    public Renderer Renderer;
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public float Duration = 1;

    public Manipulator_Workers Workers = new Manipulator_Workers();
    public Manipulator_Chain Chains = new Manipulator_Chain();

    private float time = 0;
    private float remapedTime = 0;

#if UNITY_EDITOR
    public bool Foldout = true;
    public bool WorkerFoldout = false;
#endif

    public float ElapsedTime
    {
        get
        {
            return time;
        }
    }

    public float RemapedTime
    {
        get
        {
            return remapedTime;
        }
    }

    public Manipulator()
    {
    }

    public Manipulator(Transform transform, float duration = 1)
    {
        Transform = transform;
        Duration = duration;
    }

    public Manipulator SetTransform(Transform transform, bool force = false)
    {
        if (Transform != null)
        {
            if (force)
            {
                Transform = transform;
            }
        }
        else
        {
            Transform = transform;
        }
        return this;
    }

    /// <summary>
    /// Saves the manipulator to a template to be loaded later on. 
    /// This template can either be loaded to a normal manipulator or a Monobehaviour manipulator.
    /// </summary>
    public void SaveTemplate()
    {
        Manipulator_Saver.SaveTemplate(this);
    }

    /// <summary>
    /// Loads a manipulator from a template.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="data"></param>
    /// <returns>Manipulator</returns>
    public static Manipulator LoadFromTemplate(Manipulator_Data data)
    {
        return Manipulator_Saver.LoadTemplate(data);
    }

    /// <summary>
    /// Adds a worker to the manipulator by type, and you can initialize that worker in the action. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="worker initializer">Use this action to initialize the worker</param>
    /// <returns></returns>
    public Manipulator Add<T>(Action<T> worker) where T : MWorker
    {
        var mworker = (T)Activator.CreateInstance(typeof(T), this);
        worker(mworker);
        Workers.Add(mworker);
        return this;
    }

    /// <summary>
    /// Manually add a worker to the manipulator.
    /// </summary>
    /// <param name="mWorker"></param>
    public void AddWorker(MWorker mWorker)
    {
        Workers.Add(mWorker);
    }

    /// <summary>
    /// Chains the manipulator with another manipulator that will run once the original manipulator is done.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="manipulatorInitializer"></param>
    /// <returns></returns>
    public Manipulator Chain(float duration = 1, Action<Manipulator> manipulatorInitializer = null)
    {
        var manipulator = new Manipulator(Transform, duration);
        manipulatorInitializer(manipulator);
        Chains.Add(manipulator);
        return this;
    }

    /// <summary>
    /// Manually adds a chain to the maniuplator.
    /// </summary>
    /// <param name="manipulator"></param>
    /// <returns></returns>
    public Manipulator AddChain(Manipulator manipulator)
    {
        Chains.Add(manipulator);
        return this;
    }

    /// <summary>
    /// Runs the manipulator. 
    /// </summary>
    /// <returns></returns>
    public Manipulator Run()
    {
#if UNITY_EDITOR 
        if (!Application.isPlaying)
        {
            Debug.LogError("Run() should only be called during PlayMode");
            return null;
        }
#endif
        Reset();
        Chains.SetTransform(Transform, false);
        Manipulator_Updater.Add(this);
        for (int i = 0; i < Workers.Workers.Count; i++)
        {
            Workers.Workers[i].Start();
        }
        return this;
    }


    public void Update()
    {
        time = Mathf.Clamp(time + Time.deltaTime, 0, Duration);
        remapedTime = Curve.Evaluate(time / Duration);
        for (int i = 0; i < Workers.Workers.Count; i++)
        {
            Workers.Workers[i].Update();
        }
        if (time >= Duration)
        {
            Stop();
        }
    }

    /// <summary>
    /// Stops the manipulator and Runs the Chains.
    /// </summary>
    public void Stop()
    {
        Manipulator_Updater.Remove(this);
        for (int i = 0; i < Workers.Workers.Count; i++)
        {
            Workers.Workers[i].Stop();
        }
        Chains.Run();
    }

    /// <summary>
    /// Resets the manipulator.
    /// </summary>
    public void Reset()
    {
        time = 0;
        remapedTime = 0;
        Chains.Reset();
    }

    /// <summary>
    /// Clones the manipulator to a new manipulator with same values.
    /// </summary>
    /// <param name="withWorkers"></param>
    /// <returns></returns>
    public Manipulator Clone(bool withWorkers = true)
    {
        var manipulator = new Manipulator(Transform, Duration);
        manipulator.Curve = Curve;
        // if (withWorkers)
        // {
        //     for (int i = 0; i < Workers.Workers.Count; i++)
        //     {
        //         manipulator.AddWorker(Workers.Workers[i].Clone());
        //     }
        // }
        return manipulator;
    }

    /// <summary>
    /// Converts the manipulator to a MonoBehvaiour manipulator.
    /// </summary>
    /// <param name="manipulator_Mono"></param>
    public void ToMono(Manipulator_Mono manipulator_Mono)
    {
        manipulator_Mono.manipulator = this;
        Workers.ToMono(manipulator_Mono);
        Chains.ToMono(manipulator_Mono);
    }

    /// <summary>
    /// Saves a manipulator to a JSONNode.
    /// </summary>
    /// <returns></returns>
    public JSONNode SaveJson()
    {
        JSONNode node = JSONNode.Empty;
        node["Duration"] = Duration;
        node["Curve"] = Curve.Save();
        node["Workers"] = Workers.Save();
        node["Chain"] = Chains.Save();
        return node;
    }

    /// <summary>
    /// Loads a manipulator from a JSONNode.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static Manipulator LoadJson(JSONNode node)
    {
        Manipulator manipulator = new Manipulator();
        manipulator.Duration = node["Duration"].AsFloat;
        manipulator.Curve = Save_Extensions.LoadCurve(node["Curve"]);
        manipulator.Workers.Load(manipulator, node["Workers"]);
        manipulator.Chains.Load(manipulator, node["Chain"]);
        return manipulator;
    }
}






