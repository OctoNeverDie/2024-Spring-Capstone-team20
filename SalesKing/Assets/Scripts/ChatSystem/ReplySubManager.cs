using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplySubManager
{
    public static event Action<string> OnUserReplyUpdated;
    public static event Action<string> OnGptReplyUpdated;
    public static event Action<string> OnChatDataUpdated;

    private string _UserAnswer;
    private string _GptAnswer;
    private string _GptReaction;
    public string UserAnswer
    {
        get => _UserAnswer;
        set
        {
            _UserAnswer = value;
            OnUserReplyUpdated?.Invoke(_UserAnswer);
        }
    }
    public string GptAnswer
    {
        get => _GptAnswer;
        set
        {
            _GptAnswer = value;
            OnGptReplyUpdated?.Invoke(_GptAnswer);
        }
    }

    public string GptReaction
    {
        get => _GptReaction;
        set
        {
            _GptReaction = value;
            OnChatDataUpdated?.Invoke(nameof(GptReaction));
        }
    }

    public void ClearReplyData()
    {
        UserAnswer = "";
        GptReaction = "";
    }
}
