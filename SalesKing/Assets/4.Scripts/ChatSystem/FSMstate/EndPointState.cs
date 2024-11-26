using UnityEngine;
using static Define;
/// <summary>
/// 1. turn 3번인지 확인
/// 2. summary pop up
/// 3. save 파일에 전부 저장
/// </summary>
public class EndPointState : ChatBaseState
{
    private string summary;

    public override void Enter()
    {
        SubScribeAction();
        _sendChatType = SendChatType.Endpoint;
        Chat.NpcCountUp();

        if (Chat.isEndByUser) { EndByUser(); }
        else { Exit(); }

        if (Chat.npcNum >= 3)
        {
            SaveData();
        }
    }

    public override void Exit()
    {
        ShowFront();
        UnSubScribeAction();
    }

    private void SaveData()
    {
        Chat.Eval.SaveEvaluation();
    }

    private void ShowFront()
    {
        Chat.ActivatePanel(_sendChatType);
    }

    private void EndByUser()
    {
        string user_input = "is_buy = false, 왜냐하면 상대가 나를 무시하고 갔기 때문.";
        Debug.Log($"Endpoint에서 보냄 {user_input}");
        ServerManager.Instance.GetGPTReply(Define.GameMode.Story, user_input, _sendChatType);
    }

    private void GptOutput(string type, string gpt_output)//유저가 end btn 눌렀을 때만
    {
        if (type != nameof(Chat.Reply.GptAnswer))
            return;

        ConcatReply(gpt_output);
        UpdateEvaluation();

        Exit();
    }

    private void ConcatReply(string GPTanswer)
    {
        string pattern = @"\""summary\"":\s*\""(.*?)\""";
        summary = Util.Concat(pattern, GPTanswer);
    }

    private void UpdateEvaluation()
    {
        Chat.Eval.AddEvaluation(summary, false);
    }

    private void SubScribeAction()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
        ReplySubManager.OnReplyUpdated += GptOutput;
    }

    private void UnSubScribeAction()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
    }
}
