using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LeaveState : ChatBaseState
{
    public override void Enter()
    {
        _sendChatType = SendChatType.Leave;

        Managers.Chat.ActivatePanel(SendChatType.Fail, true);
    }
}
