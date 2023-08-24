using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lib.Lerping.Test.Editors
{
    [CustomEditor(typeof(Lerper_Test))]
    public class Lerper_Test_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (Lerper_Test)target;

            if (GUILayout.Button("Lerp Position"))
            {
                script.LerpPosition();
            }

            if (GUILayout.Button("Lerp Color"))
            {
                script.LerpColor();
            }

            if (GUILayout.Button("Lerp Position Then Color"))
            {
                script.LerpPositionThenColor();
            }

            if (GUILayout.Button("Flip Position And Lerp"))
            {
                script.FlipPositionAndLerp();
            }
        }
    }
}
