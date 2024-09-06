using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSend
{
    private string _userSend = "";

    public void Init()
    {
        ChatInit();
    }

    public void SendToGPT()
    { 
        _userSend = MakeAnswer();
        Debug.Log($"userSend : {_userSend}");
        ServerManager.Instance.GetGPTReply(_userSend);
    }

    public void EndGPT()
    {
        Clear();
    }

    private string MakeAnswer()
    { 
        return $"@relationship : {VariableList.S_Relationship} / input @expecedtPrice : {VariableList.S_ExpectedPrice}, @vender input: {VariableList.S_UserAnswer}";
    }

    private string Clear()
    {
        return "clear";
    }
    private string ChatInit()
    {
        //TODO : item : too expensive, expensive, affordable
        //TODO : npc : 
        return "";
    }
}
