using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSend
{
    private string _userSend = "";
    public enum SendType
    {
        None,
        Init,
        Chat,
        Clear,
        MaxCnt
    }

    public void Init(string itemInfo, string npcInfo)
    {
        _userSend = MakeChatInit(itemInfo, npcInfo);
        SendToGPT();
    }

    public void ChatwithGPT()
    {
        _userSend = MakeAnswer();
        SendToGPT();
    }

    public void EndGPT()
    {
        _userSend = MakeClear();
        SendToGPT();
    }

    public void SendToGPT()
    {
        Debug.Log($"Send : {_userSend}");
        ServerManager.Instance.GetGPTReply(_userSend);
    }

    private string MakeAnswer()
    { 
        return $"\\SendType : {SendType.Chat} \\ @relationship : {VariableList.S_Relationship}, @expecedtPrice : {VariableList.S_ExpectedPrice}, @vender input: {VariableList.S_UserAnswer}";
    }

    private string MakeChatInit(string itemInfo, string npcInfo)
    {
        string initData = $"\\SendType : {SendType.Init}\\" + itemInfo + "\\"+ npcInfo;
        return initData;
    }
    private string MakeClear()
    {
        return $"\\SendType : {SendType.Clear}";
    }
}
