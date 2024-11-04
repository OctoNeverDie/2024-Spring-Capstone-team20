using UnityEngine;
using static Define;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData)
    {
        Managers.Chat.ReplyManager.GptAnswer = resultData;//이건 쌩
        
        string GptAnswer = GptReply(resultData);
        Managers.Chat.ReplyManager.GptReaction = GptAnswer;//이건 리액션만 따로
    }

    private string GptReply(string GPTanswer)
    {
        string pattern;

        pattern = @"\""yourReply\"":\s*\""(.*?)\"""; 
        if(Util.Concat(pattern, GPTanswer) != string.Empty)
            return Util.Concat(pattern, GPTanswer);

        return GPTanswer;
    }
}