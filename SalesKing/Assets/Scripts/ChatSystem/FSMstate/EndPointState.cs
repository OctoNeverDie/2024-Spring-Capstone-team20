using UnityEngine;
using static Define;

public class EndPointState : ChatBaseState
{
    Define.EndType _endType = EndType.None;
    public override void Enter()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
        ReplySubManager.OnReplyUpdated += GptOutput;

        _sendChatType = SendChatType.Endpoint;
        _endType = Managers.Chat._endType;

        if (_endType == EndType.clear)
        {
            Exit();
            return;
        }
       
        string input = "$"+_endType.ToString();// buy, reject, leave

        Debug.Log($"EndPointState에서 보냄 {_sendChatType}, {input}");
        ServerManager.Instance.GetGPTReply(input, _sendChatType);
    }

    public override void Exit()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;

        if(Managers.Chat.reason == 2)//gage 닳아서 2가 됐다면, gage가 0 됐다는 거 명시적으로 보여주기.
            Managers.Chat.EndTurn(-20);
        Managers.Chat.ActivatePanel(_sendChatType);
        Managers.Chat.Clear();
    }

    private void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
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
