using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator_Test_4_RuntimeLoadManipulator : MonoBehaviour
{
    public Transform TestingTransform;
    public Manipulator_Data Data;

    public void Run()
    {
        Manipulator.LoadFromTemplate(Data).SetTransform(TestingTransform).Run();
    }
}
