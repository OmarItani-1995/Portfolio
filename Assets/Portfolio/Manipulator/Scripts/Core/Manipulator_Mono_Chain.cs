using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Manipulator_Mono))]
public class Manipulator_Mono_Chain : MonoBehaviour
{
    public Manipulator_Mono manipulator_Mono;

    public Manipulator GetChain() 
    {
        return manipulator_Mono.GetManipulator();
    }
}
