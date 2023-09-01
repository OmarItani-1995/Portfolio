using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Msg_Test : MonoBehaviour
{
    void Start()
    {
        Msg.Listen<Msg_TestMessage>(OnTestMessage);
        Msg.Queue(new Msg_TestMessage()
        {
            Data = "Test Data"
        }); // Should Print Test Message 
        Msg.Remove<Msg_TestMessage>(OnTestMessage);
        Msg.Queue(new Msg_TestMessage()
        {
            Data = "Test Data"
        }); // Shouldnt Print Test Message
    }

    private void OnTestMessage(Msg_TestMessage message)
    {
        Debug.Log($"Test Message {message.Data}");
    }
}

public class Msg_TestMessage : Message
{
    public string Data = "";
}





