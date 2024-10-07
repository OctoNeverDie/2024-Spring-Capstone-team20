using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using static Define;

public class LeaveState : ChatBaseState
{
    public override void Enter()
    {
        _sendChatType = SendChatType.Leave;

        VariableList.OnVariableGptUpdated -= SaveEvaluation;
        VariableList.OnVariableGptUpdated += SaveEvaluation;
        
        ServerManager.Instance.GetGPTReply("", _sendChatType);
    }

    public override void Exit()
    {
        VariableList.OnVariableGptUpdated -= SaveEvaluation;
    }

    public void SaveEvaluation(string gpt_output)
    {
        VariableList.AddEvaluation(gpt_output);
        Exit();
    }
}
