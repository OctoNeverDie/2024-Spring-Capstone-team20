using System;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public class TemplateReceive : MonoBehaviour
{
    SendChatType nextChatType = SendChatType.None;
    public void GetGptAnswer(string resultData, SendChatType sendTypeData)
    {
        VariableList.S_GptAnswer = resultData;//이건 쌩
        if (UpdateGptReply(sendTypeData, resultData))
        {
            string GptAnswer = GptReply(sendTypeData, resultData);
            VariableList.S_GptReaction = GptAnswer;//이건 리액션만 따로
        }
    }

    private bool UpdateGptReply(SendChatType sendChatType, string resultData)
    {
        switch (sendChatType) 
        {
            case SendChatType.NpcInit:
                nextChatType = SendChatType.ChatSale;
                Managers.Chat.TransitionToState(nextChatType);
                return false;

            case SendChatType.ItemInit:
                nextChatType = SendChatType.ChatBargain;
                Managers.Chat.TransitionToState(nextChatType);
                return false;

            case SendChatType.ChatSale:
                return true;

            case SendChatType.ChatBargain:
                return true;

            case SendChatType.Endpoint:
                if (resultData == "$clear")
                    return false;
                return true;

            default:
                return false;
        }

    }
    private string GptReply(SendChatType sendChatType, string GPTanswer)
    {
        string pattern = "";

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
