using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FailState : ChatBaseState
{
    public override void Enter()
    {
        //Panel 띄우기
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
        ChatManager.ChatInstance.ActivatePanel(SendChatType.Fail);
        //TODO : Other logic to show it's fail
    }
}
