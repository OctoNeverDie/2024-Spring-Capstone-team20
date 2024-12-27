using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class ChattingState : ChatBaseState, IVariableChat
{
    const int persuMaxLimit = 3;
    const int persuMinLimit = -3;
    const int persuLimit = 6;
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

        private int _persuasion;
        [JsonProperty("persuasion")]
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
            }
        }

        [JsonProperty("emotion")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Define.Emotion emotion { get; set; }

        [JsonProperty("reason")]
        public string reason { get; set; }

        [JsonProperty("summary")]
        public string summary { get; set; }
    }
    GptResult gptResult;
    private int totalPersuasion = 0;
    private int totalTurn = 0;
    private string playerReply = "";
    
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
        if (type != nameof(Chat.Reply.UserAnswer))
            return;
        
        if (totalPersuasion >= persuMaxLimit)
        {
            user_input += "isBuy = True, 물건을 살 마음이 들었으니 yes 출력하기.";
        }
        else if (totalPersuasion <= persuMinLimit)
        {
            user_input += "isBuy = False";
        }
        else if (++totalTurn >= persuLimit)
        {
            user_input += "isBuy = you decide now";
        }

        playerReply = user_input;

        Debug.Log($"ChatBargainState에서 보냄 {user_input}");
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    public void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Chat.Reply.GptAnswer))
            return;

        if (gpt_output != "test")
        {
            UpdateReplyVariables(gpt_output);
        }
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
            Chat.updateThisSummary(gptResult.summary, gptResult.decision == Decision.yes? true : false);
            Chat.TransitionToState(Define.SendChatType.Endpoint);
        }
    }

    private void UpdateReplyVariables(string gptAnswer)
    {
        Debug.Log($"이걸 담가야해.. {gptAnswer}");
        gptAnswer = gptAnswer.Replace("\n", "").Replace("+", "").Replace("{", "").Replace("}", "").Replace("`", "").Replace("json","");
        gptAnswer = "{"+ $"\n{gptAnswer}\n"+"}";

        Debug.Log($"이걸 담가야해! {gptAnswer}");
        ServerManager.Instance.SaveChat(gptAnswer);
        string jsonPart = gptAnswer.Substring(0, gptAnswer.Length);
        try {
            gptResult = JsonConvert.DeserializeObject<GptResult>(jsonPart);
        }
        catch (JsonReaderException)
        {
            playerReply += "SystemPrompt를 잘 읽고, Json 형식에 맞춰 대답해.";
            ServerManager.Instance.GetGPTReply(playerReply, _sendChatType);
        }

        AddPersuasion(gptResult.Persuasion);
        Debug.Log($"무사히 들어왔어요!\n{gptResult.reaction}, {totalPersuasion}");
    }
    private void AddPersuasion(int persuasion)
    {
        totalPersuasion += persuasion;
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
