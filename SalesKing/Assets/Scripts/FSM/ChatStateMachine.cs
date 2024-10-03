using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChatStateMachine
{
    private ChatBaseState _currentState;

    public void SetState(ChatBaseState newState)
    {
        _currentState?.Exit();

        _currentState = newState;
        _currentState.Enter();
    }

    public void UpdateState()
    {
        _currentState?.Update();
    }
}
