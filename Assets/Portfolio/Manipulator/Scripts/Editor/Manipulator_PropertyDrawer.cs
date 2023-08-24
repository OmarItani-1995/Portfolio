using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Manipulator))]
public class Manipulator_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var manipualtor = this.fieldInfo.GetValue(property.serializedObject.targetObject) as Manipulator;

        manipualtor.Foldout = EditorGUI.Foldout(position, manipualtor.Foldout, label);
        if (manipualtor.Foldout)
        {
            EditorGUI.indentLevel++;
            var transform = property.FindPropertyRelative("Transform");
            var curve = property.FindPropertyRelative("Curve");
            var duration = property.FindPropertyRelative("Duration");

            EditorGUILayout.PropertyField(transform);
            EditorGUILayout.PropertyField(curve);
            EditorGUILayout.PropertyField(duration);

            manipualtor.WorkerFoldout = EditorGUILayout.Foldout(manipualtor.WorkerFoldout, "Workers");
            if (manipualtor.WorkerFoldout)
            {
                EditorGUI.indentLevel++;
                var array = manipualtor.Workers.Workers;
                if (array.Count == 0)
                {
                    EditorGUILayout.LabelField("No workers");
                }
                else
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        var worker = array[i];
                        var name = worker.GetType().Name;
                        if (name.Contains("MWorker_"))
                        {
                            name = name.Replace("MWorker_", "");
                        }
                        worker.Foldout = EditorGUILayout.Foldout(worker.Foldout, name);
                        if (worker.Foldout)
                        {
                            worker.OnGui(manipualtor);
                        }
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }
}
