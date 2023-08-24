using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEditor;

#if UNITY_EDITOR 
using UnityEngine;
#endif

public class MWorkerMono_MoveToPosition : MWorkerMono
{
    public Vector3 Destination;
    public Space Space;
    public override MWorker GetWorker(Manipulator manipulator)
    {
        return new MWorker_MoveToPosition(manipulator).SetDestination(Destination).UseSpace(Space);
    }
}

[System.Serializable]
public class MWorker_MoveToPosition : MWorker
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private Space space;
    private Vector3 startPosition;
    public MWorker_MoveToPosition(Manipulator manipulator) : base(manipulator)
    {
    }
    public override void Start()
    {
        startPosition = space == Space.Self ? Transform.localPosition : Transform.position;
    }

    public override void Update()
    {
        if (space == Space.World)
            Transform.position = GetPosition();
        else
            Transform.localPosition = GetPosition();
    }

    private Vector3 GetPosition()
    {
        return Vector3.Lerp(startPosition, destination, RemapedTime);
    }

    public MWorker_MoveToPosition SetDestination(Vector3 destination)
    {
        this.destination = destination;
        return this;
    }

    public MWorker_MoveToPosition UseSpace(Space space)
    {
        this.space = space;
        return this;
    }

    public override void OnGui(Manipulator manipualtor)
    {
        destination = EditorGUILayout.Vector3Field("Destination", destination);
    }

    public override void OnSave(JSONNode node)
    {
        node.Add("Destination", destination);
        node.Add("Space", (int)space);
    }

    public override void OnLoad(JSONNode node)
    {
        destination = node["Destination"];
        space = (Space)node["Space"].AsInt;
    }

    public override void ToMono(Manipulator_Mono manipulator)
    {
        var mono = manipulator.gameObject.AddComponent<MWorkerMono_MoveToPosition>();
        mono.Destination = destination;
        mono.Space = space;
    }
}