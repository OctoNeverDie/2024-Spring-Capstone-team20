using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSend
{
    private string _userSend = "";

    public void SendToGPT()
    { 
        _userSend = MakeAnswer();
        Debug.Log($"userSend : {_userSend}");
        ServerManager.Instance.GetGPTReply(_userSend);
    }

    private string MakeAnswer()
    {
        string userSend = $"@relationship : {VariableList.S_Relationship} / input @expecedtPrice : {VariableList.S_ExpectedPrice}, @affordablePrice: {VariableList.S_AffordablePrice}, @vender input: {VariableList.S_UserAnswer}";
        return userSend;
    }
}
