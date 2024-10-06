using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ChatStateMachine
{
    private ChatBaseState _currentState;

    public void SetState(ChatBaseState newState)
    {
        Debug.Log($"Go to {newState.ToString()}");
        _currentState?.Exit();

        _currentState = newState;
        _currentState.Enter();
    }

    public void UpdateState()
    {
        _currentState?.Update();
    }

    public void TransitionToState(SendChatType sendChatType)
    {
        ChatBaseState chatState = new ChatSaleState();
        switch (sendChatType)
        {
            case SendChatType.Leave:
                chatState = new LeaveState();
                break;

            case SendChatType.NpcInit:
                chatState = new NpcInitState();
                break;

            case SendChatType.ItemInit:
                chatState = new ItemInitState();
                break;

            case SendChatType.ChatSale:
                chatState = new ChatSaleState();
                break;

            case SendChatType.ChatBargain:
                chatState = new ChatBargainState();
                break;

            case SendChatType.Fail:
                chatState = new FailState();
                break;

            case SendChatType.Success:
                chatState = new SuccessState();
                break;

            default:
                chatState = new FailState();
                break;
        }

        if (_currentState is LeaveState)
            return;

        SetState(chatState);
    }
}
