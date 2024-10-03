using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChatStateMachine
{
    private ChatBase _currentState;

    public void SetState(ChatBase newState)
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
