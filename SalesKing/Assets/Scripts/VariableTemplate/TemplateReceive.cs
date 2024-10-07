using System;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public class TemplateReceive : VariableUpdate
{
    SendChatType nextChatType = SendChatType.None;
    public void GetGptAnswer(string resultData, string sendTypeData)
    {
        Debug.Log($"GPT reply : {resultData}");
        if (Enum.TryParse(sendTypeData, out SendChatType sendChatType))
        {
            if (UpdateGptReply(sendChatType, resultData))
            {
                string GptAnswer = GptReply(sendChatType, resultData);
                VariableList.S_GptAnswer = GptAnswer;
            }
        }
        else
        {
            Debug.Log("Failed to parse enum");
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
