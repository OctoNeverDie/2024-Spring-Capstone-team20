using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public class EndPointState : ChatBaseState
{
    Define.EndType _endType = EndType.None;
    public override void Enter()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
        ReplySubManager.OnReplyUpdated += GptOutput;

        _sendChatType = Define.SendChatType.Endpoint;
        _endType = Managers.Chat._endType;//None, Fail, Success, Clear
        string input = "$"+_endType.ToString();

        Debug.Log($"EndPointState에서 보냄 {_sendChatType}, {input}");
        ServerManager.Instance.GetGPTReply(input, _sendChatType);
    }

    public override void Exit()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;        
        Managers.Chat.Clear();
    }

    private void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        if (_endType == EndType.clear)
            return;

        string evaluation = ConcatReply(gpt_output);
        Debug.Log("EndPointState");
        Managers.Chat.EvalManager.AddEvaluation(evaluation);
    }

    private string ConcatReply(string GPTanswer)
    {
        string pattern = @"\""summary\"":\s*\""(.*?)\""";
        return Util.Concat(pattern, GPTanswer);
    }
}
