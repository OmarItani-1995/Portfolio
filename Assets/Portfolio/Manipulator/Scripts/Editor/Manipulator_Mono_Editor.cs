using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manipulator_Mono))]
public class Manipulator_Mono_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mono = target as Manipulator_Mono;
        if (GUILayout.Button("Run"))
        {
            mono.Run();
        }
        if (GUILayout.Button("Save Template"))
        {
            mono.SaveTemplate();
        }
        if (GUILayout.Button("Load Template"))
        {
            var path = EditorUtility.OpenFilePanel("Load template", Application.dataPath, "asset");
            var index = path.IndexOf("Assets/");
            path = path.Substring(index);
            var data = (Manipulator_Data) AssetDatabase.LoadAssetAtPath(path, typeof(Manipulator_Data));
            mono.LoadTemplate(Manipulator.LoadFromTemplate(data));
        }
    }
}
