using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public class SM_Runner : MonoBehaviour
{

}

public class SM_Runner<TState, TEffector> : SM_Runner where TState : SM_State where TEffector : SM_Effector
{
    public TState currentState;
    private TState previousState;
    public List<TEffector> effectors = new List<TEffector>();

    public void SetState(TState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        previousState = currentState;
        currentState = newState;
        currentState.Initialize(this);
        currentState.OnEnter();
    }

    public void AddEffector(TEffector effector)
    {
        effectors.Add(effector);
        effector.Initialize(this);
        effector.OnStart();
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
        for (int i = effectors.Count - 1; i >= 0; i--)
        {
            effectors[i].OnUpdate();
        }
    }

    public void RemoveEffector(TEffector effector)
    {
        effectors.Remove(effector);
    }
}



