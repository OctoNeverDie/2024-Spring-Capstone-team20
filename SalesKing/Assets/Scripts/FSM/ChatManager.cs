using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ChatManager : MonoBehaviour
{
    #region Singletone
    public static ChatManager ChatInstance { get; private set; }
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
    #endregion
    [SerializeField] private GameObject logPanel;
    [SerializeField] private GameObject gptPanel;

    private ChatStateMachine _chatStateMachine;
    void Start()
    {
        _chatStateMachine = new ChatStateMachine();
        _chatStateMachine.SetState(new NpcInitState());
    }



    //TODO : 나중에 지울 것.
    public void TestReply(String stateType, String input ="")
    {
        if (Enum.TryParse(stateType, out SendChatType sendChatType))
        {
            Debug.Log("Success");
            Debug.Log($"next stateType : {stateType}, input : {input}");
            ChatManager.ChatInstance.TransitionToState(sendChatType);
        }
        else
        {
            Debug.Log("Failed to parse enum");
        }
    }

    //TODO : state machine으로 옮길 것.
    public void TransitionToState(SendChatType sendChatType)
    {
        ChatBaseState chatState = new ChatSaleState();
        switch (sendChatType)
        {
            case SendChatType.NpcInit:
                chatState = new NpcInitState();

                break;
            case SendChatType.ChatSale:
                chatState = new ChatSaleState();
                break;

            case SendChatType.NpcNo:
                //chatState = new NpcNoState();
                break;

            case SendChatType.ItemInit:
                //chatState = new ItemInitState();
                break;
            
            default:
                //chatState = new ClearState();
                break;
        }

        _chatStateMachine.SetState(chatState);
    }
}
