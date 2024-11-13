using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChattingState : ChatBaseState, IVariableChat
{
    const int persuMaxLimit = 9;
    const int persuMinLimit = -3;
    private enum Decision
    {
        wait,
        no,
        yes
    }
    private class GptResult
    {
        public Decision decision;
        public string reaction;

        public int persuasion;
        public Define.Emotion emotion;

        public string reason;
        public string summary;

        public int totalPersuasion;
    }
    GptResult gptResult;

    public override void Enter()
    {
        SubScribeAction();

        _sendChatType = Define.SendChatType.Chatting;
        gptResult = new GptResult();
    }

    public override void Exit()
    {
        gptResult = new GptResult();
        UnSubScribeAction();
    }

    public void UserInput(string type, string user_input)
    {
        if (type != nameof(ChatManager.Instance.Reply.UserAnswer))
            return;

        if (gptResult.persuasion >= persuMaxLimit)
        {
            user_input += "isBuy = True";
        }
        else if (gptResult.persuasion <= persuMinLimit)
        {
            user_input += "isBuy = False";
        }

        Debug.Log($"ChatBargainState에서 보냄 {user_input}");
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    public void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(ChatManager.Instance.Reply.GptAnswer))
            return;

        UpdateReplyVariables(gpt_output);
        ShowFront();
        SaveData();
    }

    private void ShowFront()
    {
        //emotion에 따른 애니메이션 넣어주고
        //reply도 보여주고
        //persuasion에 따른 reason에 대한 ++, -- 보여주기

        //만약 decision이 wait이 아니라면 panel로 막기
    }

    private void SaveData()
    {
        if (gptResult.decision != Decision.wait)
        {
            ChatManager.Instance.Eval.AddEvaluation(gptResult.summary, gptResult.decision == Decision.yes);
            ChatManager.Instance.TransitionToState(Define.SendChatType.Endpoint);
        }
    }

    private void UpdateReplyVariables(string gptAnswer)
    {
        string pattern = @"(?i)(decision|yourReply|persuasion|reason|emotion|summary):\s*(.*?)(?=(\n[a-zA-Z]+:)|$)";
        var matches = Regex.Matches(gptAnswer, pattern, RegexOptions.Multiline);

        foreach (Match match in matches)
        {
            string key = match.Groups[1].Value.Trim().ToLower();
            string value = match.Groups[2].Value.Trim();

            switch (key)
            {
                case "decision":
                    if (Enum.TryParse<Decision>(value, true, out Decision decision))
                    {
                        gptResult.decision = decision;
                    }
                    break;
                case "yourreply":
                    gptResult.reaction = value;
                    break;
                case "persuasion":
                    gptResult.persuasion = int.Parse(value);
                    break;
                case "reason":
                    gptResult.reason = value;
                    break;
                case "emotion":
                    if (Enum.TryParse<Define.Emotion>(value, true, out Define.Emotion emotion))
                    {
                        gptResult.emotion = emotion;
                    }
                    break;
                case "summary":
                    gptResult.summary = value;
                    break;
                default:
                    Debug.Log("Wrong Regex match");
                    break;
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
}
