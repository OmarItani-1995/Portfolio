using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class MWorkerMono_Renderer_LerpColor : MWorkerMono
{
    public Color To { get; internal set; }
    public string ColorName { get; internal set; }

    public override MWorker GetWorker(Manipulator manipulator)
    {
        return new MWorker_Renderer_LerpColor(manipulator).SetTo(To).SetColorName(ColorName);
    }
}

public class MWorker_Renderer_LerpColor : MWorker
{
    public Color To = Color.white;
    public string ColorName = "_BaseColor";

    private Color from;
    public MWorker_Renderer_LerpColor(Manipulator manipulator) : base(manipulator)
    {
    }

    public override void Start()
    {
        from = Renderer.material.GetColor(ColorName);
    }

    public MWorker_Renderer_LerpColor SetTo(Color to)
    {
        To = to;
        return this;
    }

    public MWorker_Renderer_LerpColor SetColorName(string colorName)
    {
        ColorName = colorName;
        return this;
    }

    public override void Update()
    {
        Renderer.material.SetColor(ColorName, Color.Lerp(from, To, RemapedTime));
    }
    public override void OnGui(Manipulator manipualtor)
    {
#if UNITY_EDITOR 
        To = EditorGUILayout.ColorField("To", To);
        ColorName = EditorGUILayout.TextField("Color Name", ColorName);
#endif
    }

    public override void OnLoad(JSONNode node)
    {
        if (ColorUtility.TryParseHtmlString(node["To"].ToString(), out Color color))
        {
            To = color;
        }
        ColorName = node["ColorName"].Value;
    }

    public override void OnSave(JSONNode node)
    {
        node.Add("To", ColorUtility.ToHtmlStringRGBA(To));
        node.Add("ColorName", ColorName);
    }

    public override void ToMono(Manipulator_Mono manipulator)
    {
        var mono = manipulator.gameObject.AddComponent<MWorkerMono_Renderer_LerpColor>();
        mono.To = To;
        mono.ColorName = ColorName;
    }
}