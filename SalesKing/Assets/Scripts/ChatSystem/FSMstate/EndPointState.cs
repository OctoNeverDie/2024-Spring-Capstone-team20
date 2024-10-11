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
        Managers.Chat.ActivatePanel(_sendChatType);

        _endType = Managers.Chat._endType;//None, buy, reject, clear
        string input = "$"+_endType.ToString();
        Debug.Log($"EndPointState에서 보냄 {_sendChatType}, {input}");
        ServerManager.Instance.GetGPTReply(input, _sendChatType);
    }

    public override void Exit()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
        Managers.Turn.AddTurnAndCheckTalkTurn();
        Managers.Chat.Clear();
    }

    private void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        if (_endType == EndType.clear)
            return;

        string evaluation = ConcatReply(gpt_output);
        Managers.Chat.EvalManager.AddEvaluation(evaluation);
        Exit();
    }

    private string ConcatReply(string GPTanswer)
    {
        string pattern = @"\""summary\"":\s*\""(.*?)\""";
        return Util.Concat(pattern, GPTanswer);
    }
}
