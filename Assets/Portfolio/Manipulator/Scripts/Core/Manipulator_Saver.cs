using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

public static class Manipulator_Saver
{
    public static void SaveTemplate(Manipulator manipulator)
    {
        var path = EditorUtility.SaveFilePanel("Save template", Application.dataPath, "ManipulatorTemplate", "asset");
        if (path.Length != 0)
        {
            var node = manipulator.SaveJson();
            var data = ScriptableObject.CreateInstance<Manipulator_Data>();
            data.Data = node.ToString();
            AssetDatabase.CreateAsset(data, path.Substring(path.IndexOf("Assets")));
            AssetDatabase.SaveAssets();
        }
    }

    public static Manipulator LoadTemplate(Manipulator_Data data)
    {
        return Manipulator.LoadJson(JSON.Parse(data.Data));
    }
}

public static class Save_Extensions
{
    public static JSONNode Save(this AnimationCurve curve)
    {
        JSONNode node = JSONNode.Empty;
        JSONArray array = new JSONArray();
        var keys = curve.keys;
        for (int i = 0; i < keys.Length; i++)
        {
            array.Add(keys[i].Save());
        }
        node["Keys"] = array;
        return node;
    }

    public static JSONNode Save(this Keyframe keyframe)
    {
        JSONNode node = JSONNode.Empty;
        node["Time"] = keyframe.time;
        node["Value"] = keyframe.value;
        node["InTangent"] = keyframe.inTangent;
        node["OutTangent"] = keyframe.outTangent;
        node["InWeight"] = keyframe.inWeight;
        node["OutWeight"] = keyframe.outWeight;
        node["WeightedMode"] = (int)keyframe.weightedMode;
        return node;
    }

    public static AnimationCurve LoadCurve(JSONNode node)
    {
        var curve = new AnimationCurve();
        var array = node["Keys"].AsArray;
        for (int i = 0; i < array.Count; i++)
        {
            curve.AddKey(LoadKeyFrame(array[i]));
        }
        return curve;
    }

    public static Keyframe LoadKeyFrame(JSONNode node)
    {
        var keyframe = new Keyframe();
        keyframe.time = node["Time"].AsFloat;
        keyframe.value = node["Value"].AsFloat;
        keyframe.inTangent = node["InTangent"].AsFloat;
        keyframe.outTangent = node["OutTangent"].AsFloat;
        keyframe.inWeight = node["InWeight"].AsFloat;
        keyframe.outWeight = node["OutWeight"].AsFloat;
        keyframe.weightedMode = (WeightedMode)node["WeightedMode"].AsInt;
        return keyframe;
    }
}