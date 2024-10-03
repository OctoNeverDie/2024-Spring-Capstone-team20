using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using static Define;

public class ChatBase
{
    protected ChatStateMachine _chatStateMachine;

    protected SendChatType _sendChatType = SendChatType.None;

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    protected virtual string MakeAnswer(string user_send = ""){ return user_send; }
}
