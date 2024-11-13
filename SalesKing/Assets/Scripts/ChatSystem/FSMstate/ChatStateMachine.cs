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

    public void TransitionToState(SendChatType sendChatType)
    {
        ChatBaseState chatState;
        switch (sendChatType)
        {
            case SendChatType.ChatInit:
                chatState = new NpcInitState();
                break;

            case SendChatType.Chatting:
                chatState = new ChattingState();
                break;

            case SendChatType.Endpoint:
                chatState = new EndPointState();
                break;

            default:
                chatState = new EndPointState();
                break;
        }

        if (_currentState is EndPointState)
            return;

        SetState(chatState);
    }
}
