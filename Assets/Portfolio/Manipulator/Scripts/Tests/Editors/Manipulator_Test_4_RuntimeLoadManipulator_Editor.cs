using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manipulator_Test_4_RuntimeLoadManipulator))]
public class Manipulator_Test_4_RuntimeLoadManipulator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mono = target as Manipulator_Test_4_RuntimeLoadManipulator;
        if (GUILayout.Button("Run"))
        {
            mono.Run();
        }
    }
}
