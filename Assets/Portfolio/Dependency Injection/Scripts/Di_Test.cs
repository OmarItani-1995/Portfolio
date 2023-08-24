using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Assertions;
using UnityEngine;

public class Di_Test : MonoBehaviour
{
    void Start()
    {
        TestSingleton();
        TestInterface();
        TestGroup();
    }

    private void TestGroup()
    {
        Debug.Log("Test Group");
        Di.AddGroup<Test_Group>();
        var group = Di.GetGroup<Test_Group>();
        group.ForEach(x => x.Test());
    }

    private void TestInterface()
    {
        Debug.Log("Interface Test");
        Di.AddSingleton<IInterfaceTest, Interface_Test>();
        var interfaceTest = Di.Get<IInterfaceTest>();
        Debug.Log(interfaceTest.TestNumber);
    }

    void TestSingleton()
    {
        Debug.Log("Singleton Test");
        Di.AddSingleton<Singleton_Test>();
        var singleton = Di.Get<Singleton_Test>();
        singleton.TestNumber = 1;
        Di.AddSingleton<Singleton_Test>();
        var singleton2 = Di.Get<Singleton_Test>();
        Debug.Log($"First: {singleton.TestNumber}, Second{singleton2.TestNumber}");
    }
}

public class Singleton_Test
{
    public int TestNumber = 0;
}

public interface IInterfaceTest
{
    int TestNumber { get; set; }
}

public class Interface_Test : IInterfaceTest
{
    public int TestNumber { get; set; } = 8;
}


public abstract class Test_Group
{
    public abstract void Test();
}

public class Test_Group_1 : Test_Group
{
    public override void Test()
    {
        Debug.Log("Test Group: Class 1");
    }
}

public class Test_Group_2 : Test_Group
{
    public override void Test()
    {
        Debug.Log("Test Group: Class 2");
    }
}

