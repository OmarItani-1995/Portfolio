using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator_Mono : MonoBehaviour
{
    public Manipulator manipulator;

    public void Run()
    {
        SetUpManipulator(manipulator);
        manipulator.Run();
    }

    public Manipulator GetManipulator()
    {
        var manipulator = this.manipulator.Clone(false);
        SetUpManipulator(manipulator);
        return manipulator;
    }

    private void SetUpManipulator(Manipulator manipulator)
    {
        var monoWorkers = GetComponents<MWorkerMono>();
        foreach (var monoWorker in monoWorkers)
        {
            manipulator.AddWorker(monoWorker.GetWorker(manipulator));
        }
        var chains = GetComponents<Manipulator_Mono_Chain>();
        foreach (var chain in chains)
        {
            manipulator.AddChain(chain.GetChain());
        }
    }

    public void SaveTemplate()
    {
        var manipulator = this.manipulator.Clone(false);
        SetUpManipulator(manipulator);
        manipulator.SaveTemplate();
    }

    public void LoadTemplate(Manipulator manipulator)
    {
        var monos = GetComponents<MWorkerMono>();
        foreach (var mono in monos) 
        {
            DestroyImmediate(mono);
        }
        var chains = GetComponents<Manipulator_Mono_Chain>();
        foreach (var chain in chains)
        {
            DestroyImmediate(chain);
        }
        manipulator.ToMono(this);
    }
}

