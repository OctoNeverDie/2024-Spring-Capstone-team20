using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class ChattingState : ChatBaseState, IVariableChat
{
    const int persuMaxLimit = 8;
    const int persuMinLimit = -5;
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
        private int _persuasion;
        public int Persuasion {
            get => _persuasion;
            set
            {
                // Convert value to a string only once and avoid infinite recursion
                string stringValue = (value.ToString());

                if (stringValue.StartsWith("+"))
                {
                    stringValue = stringValue.TrimStart('+');
                    _persuasion = int.TryParse(stringValue, out int parsedValue) ? parsedValue : value;
                }
                else 
                {
                    _persuasion = (int)value;
                }
                totalPersuasion += _persuasion;
                Debug.Log($"더했다, {totalPersuasion}");
            }
        }

        [JsonProperty("emotion")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Emotion emotion { get; set; }

        [JsonProperty("reason")]
        public string reason { get; set; }

        [JsonProperty("summary")]
        public string summary { get; set; }

        public int totalPersuasion = 0;
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

        if (gptResult.totalPersuasion >= persuMaxLimit)
        {
            user_input += "isBuy = True";
        }
        else if (gptResult.totalPersuasion <= persuMinLimit)
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
        gptAnswer = gptAnswer.Replace("\n", "").Replace("+", "").Replace("{", "").Replace("}", "").Replace("`", "").Replace("json","");
        gptAnswer = "{"+ $"\n{gptAnswer}\n"+"}";

        Debug.Log($"이걸 담가야해! {gptAnswer}");
        string jsonPart = gptAnswer.Substring(0, gptAnswer.Length);
        gptResult = JsonConvert.DeserializeObject<GptResult>(jsonPart);

        Debug.Log($"무사히 들어왔어요!\n{gptResult.reaction}, {gptResult.totalPersuasion}");
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