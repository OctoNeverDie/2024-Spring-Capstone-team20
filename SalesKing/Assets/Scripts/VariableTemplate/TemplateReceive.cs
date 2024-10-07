using System;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public class TemplateReceive : MonoBehaviour
{
    SendChatType nextChatType = SendChatType.None;
    public void GetGptAnswer(string resultData, SendChatType sendTypeData)
    {
        if (UpdateGptReply(sendTypeData, resultData))
        {
            string GptAnswer = GptReply(sendTypeData, resultData);
            VariableList.S_GptAnswer = GptAnswer;
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

            case SendChatType.ChatSale:
                VariableList.S_GptAnswer = resultData;
                return false;

            case SendChatType.ItemInit:
                nextChatType = SendChatType.ChatBargain;
                Managers.Chat.TransitionToState(nextChatType);
                return false;

            case SendChatType.ChatBargain:
                VariableList.S_GptAnswer = resultData;
                return false;

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

            if (match.Success) return match.Groups[1].Value;
        }

        return GPTanswer;
    }


}
