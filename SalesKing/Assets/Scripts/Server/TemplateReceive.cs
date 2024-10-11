using UnityEngine;
using static Define;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData, SendChatType sendTypeData)
    {
        Managers.Chat.ReplyManager.GptAnswer = resultData;//이건 쌩

        //reaction 할 건가?
        if (UpdateGptReply(sendTypeData, resultData))
        {
            string GptAnswer = GptReply(sendTypeData, resultData);
            Managers.Chat.ReplyManager.GptReaction = GptAnswer;//이건 리액션만 따로
        }

        if (sendTypeData == SendChatType.Endpoint)
        {
            Managers.Chat.ActivatePanel(SendChatType.Endpoint);
        }
    }

    private bool UpdateGptReply(SendChatType sendChatType, string resultData)
    {
        switch (sendChatType) 
        {
            case SendChatType.NpcInit:
                Managers.Chat.TransitionToState(SendChatType.ChatSale);
                return false;

            case SendChatType.ItemInit:
                Managers.Chat.TransitionToState(SendChatType.ChatBargain);
                return false;

            case SendChatType.ChatSale:
                return true;

            case SendChatType.ChatBargain:
                return true;

            case SendChatType.Endpoint:
                return false;

            default:
                return false;
        }

    }
    private string GptReply(SendChatType sendChatType, string GPTanswer)
    {
        string pattern;

        if (sendChatType == SendChatType.ChatSale)
        {
            pattern = @"\""yourReply\"":\s*\""(.*?)\""";
            return Util.Concat(pattern, GPTanswer);
        }
        else if ((sendChatType == SendChatType.ChatBargain) || (sendChatType == SendChatType.Endpoint))
        {
            pattern = @"\""reaction\"":\s*\""(.*?)\"""; 
            return Util.Concat(pattern, GPTanswer);
        }

        return GPTanswer;
    }
}
