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

        ReplySubManager.OnReplyUpdated -= GptOutput;
        ReplySubManager.OnReplyUpdated += GptOutput;

        this._endType = Managers.Chat._endType;
        string input = "$"+_endType.ToString();

        Debug.Log($"EndPointState에서 보냄 {_sendChatType}, {input}");
        ServerManager.Instance.GetGPTReply(input, _sendChatType);

    }

    public override void Exit()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;

        _gptResult = new GptResult();
        Managers.Chat.Clear();
    }

    private void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        ConcatReply(gpt_output);
        Debug.Log("EndPointState");
        Managers.Chat.EvalManager.AddEvaluation(_gptResult.evaluation);
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
