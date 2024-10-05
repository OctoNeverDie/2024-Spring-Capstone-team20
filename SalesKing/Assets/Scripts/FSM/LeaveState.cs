using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LeaveState : ChatBaseState
{
    public override void Enter()
    {
        _sendChatType = SendChatType.Leave;

        ChatManager.ChatInstance.ActivatePanel(SendChatType.Fail, true);
    }
}
