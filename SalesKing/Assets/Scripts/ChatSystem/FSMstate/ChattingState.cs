using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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

        if (isState == State.Wait)
            CheckState();

        UpdateTurn();
        ReactToState();
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
            string[] sections = gptAnswer.Split(new string[] { "decision", "yourReply", "summary" }, StringSplitOptions.None);
            if (sections.Length > 2)
            {
                string decisionStr = sections[0].Trim();
                gptResult.summary = sections[2].Trim();
            }
            else
                Debug.Log("Some Summary is 누락");

            Managers.Chat.EvalManager.AddEvaluation(gptResult.summary);

            string actionValue = sections2[3].Trim();

            if (actionValue.Contains("bought"))
            {
                Debug.Log("샀다구여.");
                float finalPrice = GetFloat(sections2[4]);
                _gptResult._npcSuggest = finalPrice;
                _gptResult._userSuggest = finalPrice;
                StateFailSuccess(State.Success, 3, EndType.clear);
            }
            else if (actionValue.Contains("notBought"))
            {
                Debug.Log("안 샀다구여.");
                StateFailSuccess(State.Fail, 1, EndType.clear);
            }
        }
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

    private enum Emotion
    {
        worst,
        bad,
        normal,
        good,
        best
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
        public Emotion emotion;

        public string summary;

        public GptResult()
        { 
            decision = Decision.wait;
            persuasion = 0;
            emotion = Emotion.normal;
            summary = "";
        }
    }
}
