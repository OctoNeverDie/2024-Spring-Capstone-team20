using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FailState : ChatBaseState
{
    public override void Enter()
    {
        
        Managers.Chat.ActivatePanel(SendChatType.Fail);

        if (VariableList.CheckEvaluationIsAlready())
        {
            return;
        }

        VariableList.OnVariableGptUpdated -= SaveEvaluation;
        VariableList.OnVariableGptUpdated += SaveEvaluation;


        ServerManager.Instance.GetGPTReply("$reject", _sendChatType);
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
        Managers.Chat.ActivatePanel(SendChatType.Fail);
        VariableList.OnVariableGptUpdated -= SaveEvaluation;
        //TODO : add other end action
    }
    public void SaveEvaluation(string gpt_output)
    {
        VariableList.AddEvaluation(gpt_output);
        Exit();
    }
}
