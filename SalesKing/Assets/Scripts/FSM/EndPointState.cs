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
        ServerManager.Instance.GetGPTReply(input, _sendChatType);

    }

    public override void Exit()
    {
        VariableList.OnVariableGptUpdated -= GptOutput;

        _gptResult = new GptResult();
    }
    private void GptOutput(string gpt_output)
    {
        ConcatReply(gpt_output);
        VariableList.AddEvaluation(_gptResult.evaluation);

        //성공/실패 리액션 보여줌
        Managers.Chat.UpdatePanel(_gptResult.reaction);
        
        //아이템 팔기 성공/실패 패널 뜬다.
        Managers.Chat.ActivatePanel(_sendChatType);
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
