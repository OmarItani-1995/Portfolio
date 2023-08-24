using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manipulator_Test_1_RuntimeManipulator))]
public class Maniuplator_Test_1_RuntimeManipulator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = target as Manipulator_Test_1_RuntimeManipulator;

        if (GUILayout.Button("Run"))
        {
            script.Run();
        }
    }
}
