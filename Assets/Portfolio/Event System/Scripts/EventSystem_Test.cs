using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem_Test : MonoBehaviour
{
    void Start()
    {
        EventSystem.FireEvent(new TestEvent { Name = "Test Event" });
    }
}

public class TestEvent
{
    public string Name { get; set; }
}

public class TestEventListener_First : Event_Listener<TestEvent>
{
    public override void OnEvent(TestEvent ev)
    {
        Debug.Log($"First Event Listener: {ev.Name}");
    }
}

public class TestEventListener_Second : Event_Listener<TestEvent>
{
    public override void OnEvent(TestEvent ev)
    {
        Debug.Log($"Second Event Listener: {ev.Name}");
    }
}

