using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSend
{
    private string _userSend = "";
    public enum SendChatType
    {
        None,
        Init,
        Chat,
        Clear,
        MaxCnt
    }

    public void Init(string npcInfo)
    {
        _userSend = MakeChatInit(npcInfo);
        SendToGPT(SendChatType.Init);
    }

    public void ChatwithGPT()
    {
        _userSend = MakeAnswer();
        SendToGPT(SendChatType.Chat);
    }

    public void EndGPT()
    {
        _userSend = MakeClear();
        SendToGPT(SendChatType.Clear);
    }

    public void SendToGPT(SendChatType sendChatTypeFrom)
    {
        Debug.Log($"Send : {_userSend}");
        ServerManager.Instance.GetGPTReply(_userSend, sendChatTypeFrom);
    }

    private string MakeAnswer()
    { 
        return $"@relationship : {VariableList.S_Relationship}, @expecedtPrice : {VariableList.S_ExpectedPrice}, @vender input: {VariableList.S_UserAnswer}";
    }

    private string MakeChatInit(string npcInfo)
    {
        string initData = $"\n {npcInfo}";
        //string initData = $"\n {itemInfo} \n {npcInfo}";
        return initData;
    }
    private string MakeClear()
    {
        return "Clear";
    }
}
