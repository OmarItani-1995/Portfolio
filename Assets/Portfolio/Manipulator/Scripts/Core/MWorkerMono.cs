using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Manipulator_Mono))]
public abstract class MWorkerMono : MonoBehaviour
{
    public abstract MWorker GetWorker(Manipulator manipulator);
}