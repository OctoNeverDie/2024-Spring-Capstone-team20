using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SuccessState : ChatBaseState
{
    public override void Enter()
    {

        _sendChatType = Define.SendChatType.Success;

        VariableList.OnVariableGptUpdated -= GptOutput;
        VariableList.OnVariableGptUpdated += GptOutput;

        ServerManager.Instance.GetGPTReply("\\\"reaction\\\": \\\"Generate a response where the player asks why the item is so expensive and expresses doubt. The tone should reflect the character's personality traits (e.g., timid, anxious, or assertive).\\\",\r\n        \\\"summary\\\" : \\\"최악의 거래\\\"", SendChatType.Success);
        //TODO : variableList.currentNPC의 itemPrice에 합의요금 업데이트
        //유저의 bill system에 variableList.itemPrice에 업데이트
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

        //아이템 팔기 성공 패널 뜬다.
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
