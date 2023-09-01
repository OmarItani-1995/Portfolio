using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SM_State : ScriptableObject
{
    public abstract void Initialize(SM_Runner runner);

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
    }
}

public class SM_State<TRunner> : SM_State where TRunner : SM_Runner
{
    protected TRunner runner;

    public override void Initialize(SM_Runner runner)
    {
        runner = (TRunner)runner;
    }
}