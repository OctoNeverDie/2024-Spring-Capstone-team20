using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSaleState : ChatBaseState, IVariableChat
{
    public override void Enter()
    {
        VariableList.OnVariableUserUpdated += Input;
        VariableList.OnVariableGptUpdated += Output;
    }

    public void Input(string user_input)
    {
        //TODO : test
        VariableList.S_GptAnswer = "그래, 사지 뭐.";
        //ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }

    public void Output(string gpt_output)
    {
        ChatManager.ChatInstance.TestReply("ChatSaleState", gpt_output);
    }

    public override void Update()
    {
        
    }
}
