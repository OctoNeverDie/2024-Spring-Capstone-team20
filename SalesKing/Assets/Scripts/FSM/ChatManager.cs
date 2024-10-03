using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ChatManager : MonoBehaviour
{   public static ChatManager ChatInstance { get; private set; }

    private void Awake()
    {
        if (ChatInstance == null)
        {
            ChatInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private ChatStateMachine _chatStateMachine;
    void Start()
    {
        _chatStateMachine = new ChatStateMachine();
        _chatStateMachine.SetState(new NpcInitState());
    }
    public void TestReply(String stateType, String input ="")
    {
        if (Enum.TryParse(stateType, out SendChatType sendChatType))
        {
            Debug.Log("Success");
            Debug.Log($"stateType : {stateType}, input : {input}");
            ChatManager.ChatInstance.TransitionToState(sendChatType);
        }
        else
        {
            Debug.Log("Failed to parse enum");
        }
    }

    public void TransitionToState(SendChatType sendChatType)
    {
        ChatBaseState chatState = new ChatSaleState();
        switch (sendChatType)
        {
            case SendChatType.NpcInit:
                Debug.Log("NPCInit Done");
                chatState = new ChatSaleState();

                break;
            default:
                //chatState = new ClearState();
                break;
        }

        _chatStateMachine.SetState(chatState);
    }
}
