using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LeaveState : ChatBaseState
{
    public override void Enter()
    {
        VariableList.OnVariableGptUpdated += SaveEvaluation;

        _sendChatType = SendChatType.Leave;

        ChatManager.ChatInstance.ActivatePanel(_sendChatType);
        
        string clear = "Make Final Evaluation";
        //ServerManager.Instance.GetGPTReply(clear, _sendChatType);
    }

    public override void Exit()
    {
        VariableList.OnVariableGptUpdated -= SaveEvaluation;
        //TODO : add other end action
    }

    public void SaveEvaluation(string gpt_output)
    {
        VariableList.AddEvaluation(gpt_output);
        Exit();
    }


}
