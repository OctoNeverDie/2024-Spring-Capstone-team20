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
        Debug.Log("?????????");
        SubScribeAction();
        _sendChatType = SendChatType.Endpoint;
        Chat.NpcCountUp();

        if (Chat.npcNum >= 3)
        {
            SaveData();
        }

        ShowFront();
    }

    public override void Exit()
    {
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

    private void GptOutput(string type, string gpt_output)//유저가 end btn 눌렀을 때만
    {
        if (type != nameof(Chat.Reply.GptAnswer))
            return;

        ConcatReply(gpt_output);
        UpdateEvaluation();

        ShowFront();
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
