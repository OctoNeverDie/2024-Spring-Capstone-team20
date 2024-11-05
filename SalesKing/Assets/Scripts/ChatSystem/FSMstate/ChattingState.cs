using System;
using UnityEngine;

public class ChattingState : ChatBaseState, IVariableChat
{
    GptResult gptResult;
    public override void Enter()
    {
        SubScribeAction();
        _sendChatType = Define.SendChatType.Chatting;
        gptResult = new GptResult();
    }

    public override void Exit()
    {
        UnSubScribeAction();
    }

    public void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        ConcatReply(gpt_output);

        if (gptResult.decision == Decision.wait)
        {
            Managers.Chat.EvalManager.UpdateNpcDict(gptResult.emotion, gptResult.reason);
        }
        else
        { 
            //값 업데이트 하기, UI 팝업
        }
    }


    public void UserInput(string type, string user_input)
    {
        if (type != nameof(Managers.Chat.ReplyManager.UserAnswer))
            return;

        Debug.Log($"ChatBargainState에서 보냄 {user_input}");
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    private void ConcatReply(string gptAnswer)
    {
        if (gptAnswer.Contains("summary"))
        {
            ChatConcat(true, gptAnswer);
            return;
        }

        ChatConcat(false, gptAnswer);
    }

    private void ChatConcat(bool isEnd, string gptAnswer)
    {
        if (isEnd)
        {
            // 'decision', 'yourReply', 'summary' 키워드 사이의 값을 추출합니다.
            string decisionValue = GetValueBetween(gptAnswer, "decision", "yourReply");
            string yourReplyValue = GetValueBetween(gptAnswer, "yourReply", "summary");
            string summaryValue = GetValueAfter(gptAnswer, "summary");

            if (string.IsNullOrEmpty(decisionValue) || string.IsNullOrEmpty(summaryValue))
            {
                Debug.Log("Not enough elements");
                return;
            }

            gptResult.summary = summaryValue.Trim();
            Debug.Log($"gptResult decision {gptResult.decision}");

            if (decisionValue.Contains("yes"))
                Managers.Chat.EvalManager.AddEvaluation(gptResult.summary, true);
            else
                Managers.Chat.EvalManager.AddEvaluation(gptResult.summary, false);

            return;
        }
        else
        {
            // 'persuasion', 'reason', 'emotion' 키워드 사이의 값을 추출합니다.
            string persuasionValue = GetValueBetween(gptAnswer, "persuasion", "reason");
            string reasonValue = null;
            string emotionValue = null;

            if (gptAnswer.Contains("emotion"))
            {
                reasonValue = GetValueBetween(gptAnswer, "reason", "emotion");
                emotionValue = GetValueAfter(gptAnswer, "emotion");
            }
            else
            {
                emotionValue = GetValueAfter(gptAnswer, "reason");
            }

            if (string.IsNullOrEmpty(persuasionValue) || string.IsNullOrEmpty(emotionValue))
            {
                Debug.Log("Not enough elements");
                return;
            }

            gptResult.decision = Decision.wait;

            int persuasion;
            if (int.TryParse(persuasionValue, out persuasion))
            {
                gptResult.persuasion = persuasion;
            }
            else
            {
                Debug.Log("Failed to parse persuasion value");
                return;
            }

            if (!string.IsNullOrEmpty(reasonValue))
            {
                gptResult.reason = reasonValue.Trim();
            }

            gptResult.emotion = (Define.Emotion)Enum.Parse(typeof(Define.Emotion), emotionValue.Trim(), true);

            return;
        }
    }

    // 문자열에서 두 키워드 사이의 값을 추출하는 헬퍼 메서드
    private string GetValueBetween(string source, string start, string end)
    {
        int startIndex = source.IndexOf(start);
        if (startIndex == -1)
            return null;

        startIndex += start.Length;
        int endIndex = source.IndexOf(end, startIndex);
        if (endIndex == -1)
            return null;

        return source.Substring(startIndex, endIndex - startIndex).Trim();
    }

    // 문자열에서 특정 키워드 이후의 값을 추출하는 헬퍼 메서드
    private string GetValueAfter(string source, string start)
    {
        int startIndex = source.IndexOf(start);
        if (startIndex == -1)
            return null;

        startIndex += start.Length;
        return source.Substring(startIndex).Trim();
    }

    private void SubScribeAction()
    {
        ReplySubManager.OnReplyUpdated -= UserInput;
        ReplySubManager.OnReplyUpdated -= GptOutput;

        ReplySubManager.OnReplyUpdated += UserInput;
        ReplySubManager.OnReplyUpdated += GptOutput;
    }
    private void UnSubScribeAction()
    {
        ReplySubManager.OnReplyUpdated -= UserInput;
        ReplySubManager.OnReplyUpdated -= GptOutput;
    }

    private enum Decision
    {
        wait,
        no,
        yes
    }

    private class GptResult
    {
        public Decision decision;

        public int persuasion;
        public Define.Emotion emotion;

        public string reason;
        public string summary;

        public GptResult()
        { 
            decision = Decision.wait;
            persuasion = 0;
            emotion = Define.Emotion.normal;
            reason = "";
            summary = "";
        }
    }
}
