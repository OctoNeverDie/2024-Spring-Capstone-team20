using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FailState : ChatBaseState
{
    public override void Enter()
    {
        _sendChatType = Define.SendChatType.Fail;

        VariableList.OnVariableGptUpdated -= GptOutput;
        VariableList.OnVariableGptUpdated += GptOutput;


        //ServerManager.Instance.GetGPTReply("$reject", _sendChatType);
        ServerManager.Instance.GetGPTReply("\\\"reaction\\\": \\\"Generate a response where the player asks why the item is so expensive and expresses doubt. The tone should reflect the character's personality traits (e.g., timid, anxious, or assertive).\\\",\r\n        \\\"summary\\\" : \\\"최악의 거래\\\"", _sendChatType);
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

        //실패 리액션 보여줌
        Managers.Chat.UpdatePanel(_gptResult.reaction);

        //아이템 팔기 실패 패널 뜬다.
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
