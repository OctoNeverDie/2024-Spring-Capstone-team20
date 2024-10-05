using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FailState : ChatBaseState
{
    bool evaluationAlreay = false;
    public override void Enter()
    {
        ChatManager.ChatInstance.ActivatePanel(SendChatType.Fail);

        if (VariableList.CheckEvaluationIsAlready())
        {
            return;
        }

        VariableList.OnVariableGptUpdated -= SaveEvaluation;
        VariableList.OnVariableGptUpdated += SaveEvaluation;

        string clear = "Make Final Evaluation";
        //ServerManager.Instance.GetGPTReply(clear, _sendChatType);
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
        VariableList.OnVariableGptUpdated -= SaveEvaluation;
        //TODO : add other end action
    }
    public void SaveEvaluation(string gpt_output)
    {
        VariableList.AddEvaluation(gpt_output);
        Exit();
    }
}
