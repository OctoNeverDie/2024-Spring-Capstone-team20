using System;

public class ReplySubManager
{
    public static event Action<string, string> OnReplyUpdated;

    private string _UserAnswer;
    private string _GptAnswer;
    private string _GptReaction;
    public string UserAnswer
    {
        get => _UserAnswer;
        set
        {
            _UserAnswer = value;
            OnReplyUpdated?.Invoke(nameof(UserAnswer), _UserAnswer);
        }
    }
    public string GptAnswer
    {
        get => _GptAnswer;
        set
        {
            _GptAnswer = value;
            OnReplyUpdated?.Invoke(nameof(GptAnswer), GptAnswer);
        }
    }

    public string GptReaction
    {
        get => _GptReaction;
        set
        {
            _GptReaction = value;
            OnReplyUpdated?.Invoke(nameof(GptReaction), GptReaction);
        }
    }

    public void ClearReplyData()
    {
        UserAnswer = "";
        GptReaction = "";
    }
}
