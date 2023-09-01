using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SM_Effector
{
    public abstract void Initialize(SM_Runner runner);

    public virtual void OnUpdate()
    {
    }

    public virtual void OnStart()
    {
    }
}

[System.Serializable]
public class SM_Effector<TRunner> : SM_Effector where TRunner : SM_Runner
{
    protected TRunner runner;

    public override void Initialize(SM_Runner runner)
    {
        runner = (TRunner)runner;
    }
}
