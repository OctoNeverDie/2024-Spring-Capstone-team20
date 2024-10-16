using UnityEngine;
using static Define;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData, SendChatType sendTypeData)
    {
        Managers.Chat.ReplyManager.GptAnswer = resultData;//이건 쌩
        
        string GptAnswer = GptReply(sendTypeData, resultData);
        Managers.Chat.ReplyManager.GptReaction = GptAnswer;//이건 리액션만 따로
    }

    private string GptReply(SendChatType sendChatType, string GPTanswer)
    {
        string pattern;

        if (sendChatType != SendChatType.NpcInit) //Npcinit 제외 모두 정제 필요
        {
            pattern = @"\""reaction\"":\s*\""(.*?)\"""; 
            if(Util.Concat(pattern, GPTanswer) != string.Empty)
                return Util.Concat(pattern, GPTanswer);
        }

        return GPTanswer;
    }
}