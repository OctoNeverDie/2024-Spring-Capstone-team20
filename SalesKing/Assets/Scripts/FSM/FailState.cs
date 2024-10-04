using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FailState : ChatBaseState
{
    public override void Enter()
    {
        ChatManager.ChatInstance.ActivatePanel(SendChatType.Fail);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Exit();
        }
    }

    public override void Exit() 
    {
        //TODO : Other logic to show it's fail
        ChatManager.ChatInstance.ActivatePanel(SendChatType.Fail);
    }
}
