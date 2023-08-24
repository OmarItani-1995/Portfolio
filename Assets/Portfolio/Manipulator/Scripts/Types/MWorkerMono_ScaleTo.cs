using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MWorkerMono_ScaleTo : MWorkerMono
{
    public Vector3 EndScale;
    public override MWorker GetWorker(Manipulator manipulator)
    {
        return new MWorker_ScaleTo(manipulator).SetEndScale(EndScale);
    }
}

[System.Serializable]
public class MWorker_ScaleTo : MWorker
{
    [SerializeField] private Vector3 endScale;
    private Vector3 startScale;
    public MWorker_ScaleTo(Manipulator manipulator) : base(manipulator)
    {
    }

    public override void Start()
    {
        startScale = Transform.localScale;
    }

    public override void Update()
    {
        Transform.localScale = Vector3.Lerp(startScale, endScale, RemapedTime);
    }

    public MWorker_ScaleTo SetEndScale(Vector3 endScale)
    {
        this.endScale = endScale;
        return this;
    }

    public override void OnGui(Manipulator manipualtor)
    {
        endScale = EditorGUILayout.Vector3Field("End Scale", endScale);
    }

    public override void OnSave(JSONNode node)
    {
        node.Add("EndScale", endScale);
    }

    public override void OnLoad(JSONNode node)
    {
        endScale = node["EndScale"];
    }

    public override void ToMono(Manipulator_Mono manipulator)
    {
        var mono = manipulator.gameObject.AddComponent<MWorkerMono_ScaleTo>();
        mono.EndScale = endScale;
    }
}
