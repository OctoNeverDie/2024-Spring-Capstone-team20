using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EndPointState : ChatBaseState
{
    Define.EndType _endType = EndType.None;
    public override void Enter()
    {
        _sendChatType = Define.SendChatType.Endpoint;

        VariableList.OnVariableGptUpdated -= GptOutput;
        VariableList.OnVariableGptUpdated += GptOutput;

        this._endType = Managers.Chat._endType;
        string input = "$"+_endType.ToString();

        Debug.Log($"EndPointState에서 보냄 {_sendChatType}, {input}");
        ServerManager.Instance.GetGPTReply(input, _sendChatType);

    }

    public override void Exit()
    {
        VariableList.OnVariableGptUpdated -= GptOutput;

        _gptResult = new GptResult();
        Debug.Log("Clear!!!!!1111");
        Managers.Chat.Clear();
    }

    private void GptOutput(string gpt_output)
    {
        ConcatReply(gpt_output);
        VariableList.AddEvaluation(_gptResult.evaluation);
    }

    private struct GptResult
    {
        public string reaction;
        public string evaluation;
    }

    GptResult _gptResult = new GptResult();
    private void ConcatReply(string gptAnswer)
    {
        string[] sections = gptAnswer.Split(new string[] { "reaction", "summary" }, StringSplitOptions.None);

        if (sections.Length > 2)
        {
            _gptResult.reaction = sections[1];
            _gptResult.evaluation = sections[2];
        }
    }
}
