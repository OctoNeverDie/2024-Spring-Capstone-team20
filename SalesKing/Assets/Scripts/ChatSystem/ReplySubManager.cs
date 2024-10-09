using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplySubManager
{
    public static event Action<string> OnVariableUserUpdated;
    public static event Action<string> OnVariableGptUpdated;
    public static event Action<string> OnVariableEtcUpdated;

    private string _s_UserAnswer;
    private string _s_GptAnswer;
    private string _s_GptReaction;
    public string S_UserAnswer
    {
        get => _s_UserAnswer;
        set
        {
            _s_UserAnswer = value;
            OnVariableUserUpdated?.Invoke(_s_UserAnswer);
        }
    }
    public string S_GptAnswer
    {
        get => _s_GptAnswer;
        set
        {
            _s_GptAnswer = value;
            OnVariableGptUpdated?.Invoke(_s_GptAnswer);
        }
    }

    public string S_GptReaction
    {
        get => _s_GptReaction;
        set
        {
            _s_GptReaction = value;
            OnVariableEtcUpdated?.Invoke(nameof(S_GptReaction));
        }
    }

    public void ClearReplyData()
    {
        _s_UserAnswer = "";
        _s_GptAnswer = "";
    }
}
