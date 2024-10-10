using System;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData, SendChatType sendTypeData)
    {
        Managers.Chat.ReplyManager.GptAnswer = resultData;//이건 쌩
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
                if (resultData == "$clear")//npc가 chat sale에서 no 했을 때, 따로 summary 필요 없음
                    return false;
                return true;

            default:
                return false;
        }

    }
    private string GptReply(SendChatType sendChatType, string GPTanswer)
    {
        string pattern;

        if (sendChatType == SendChatType.ChatSale)
        {
            pattern = @"\""yourReply\"": \""(.*?)\""";
            Match match = Regex.Match(GPTanswer, pattern);

            if (match.Success)
                return match.Groups[1].Value;
        }
        else if ((sendChatType == SendChatType.ChatBargain) || (sendChatType == SendChatType.Endpoint))
        {
            pattern = @"\""reaction\"": \""(.*?)\""";
            Match match = Regex.Match(GPTanswer, pattern);

            if (match.Success)
                return match.Groups[1].Value;
        }

        return GPTanswer;
    }


}
