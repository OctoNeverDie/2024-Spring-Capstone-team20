using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class ChattingState : ChatBaseState, IVariableChat
{
    const int persuMaxLimit = 9;
    const int persuMinLimit = -3;
    public enum Decision
    {
        wait,
        no,
        yes
    }
    public class GptResult
    {
        [JsonProperty("decision")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Decision decision { get; set; }

        [JsonProperty("yourReply")]
        public string reaction { get; set; }

        [JsonProperty("persuasion")]
        public int persuasion { get; set; }

        [JsonProperty("emotion")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Emotion emotion { get; set; }

        [JsonProperty("reason")]
        public string reason { get; set; }

        [JsonProperty("summary")]
        public string summary { get; set; }

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
        UpdateEvaluation();
    }

    private void ShowFront()
    {
        Chat.ActivatePanel(_sendChatType, gptResult);
    }

    private void UpdateEvaluation()
    {
        if (gptResult.decision != Decision.wait)
        {
            ChatManager.Instance.Eval.AddEvaluation(gptResult.summary, gptResult.decision == Decision.yes);
            ChatManager.Instance.TransitionToState(Define.SendChatType.Endpoint);
        }
    }

    private void UpdateReplyVariables(string gptAnswer)
    {
        Debug.Log($"이걸 담가야해.. {gptAnswer}");
        int startIndex = gptAnswer.IndexOf("{");
        int endIndex = gptAnswer.LastIndexOf("}");
        string jsonPart="";

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            jsonPart = gptAnswer.Substring(startIndex, endIndex - startIndex + 1);
            Debug.Log($"이걸 담가야해 {jsonPart}");
        }
        else 
        {
            jsonPart = gptAnswer.Substring(0, gptAnswer.Length);
            Debug.Log($"{startIndex}, {endIndex}"); 
        } 

        gptResult = JsonConvert.DeserializeObject<GptResult>(jsonPart);

        Debug.Log($"무사히 들어왔어요!\n{gptResult.reaction}");
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
