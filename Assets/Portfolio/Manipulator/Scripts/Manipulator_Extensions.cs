using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manipulator_Extensions
{
    #region Manipulator
    public static Manipulator Manipulate(this GameObject gameObject, float duration = 1)
    {
        return gameObject.transform.Manipulate(duration);
    }

    public static Manipulator Manipulate(this Transform transform, float duration = 1, Action<Manipulator> action = null)
    {
        var manipualtor = new Manipulator(transform, duration);
        action(manipualtor);
        return manipualtor;
    }
    #endregion

    #region Manipulator_Worker
    public static Manipulator Run(this MWorker worker) 
    {
        return worker.Manipulator.Run();
    }
    #endregion
}
