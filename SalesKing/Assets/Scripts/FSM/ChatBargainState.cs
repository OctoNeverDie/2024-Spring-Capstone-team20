using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using static Define;

public class ChatBargainState : ChatBaseState, IVariableChat
{
    const int TurnInit = 8;
    private struct GptResult
    {
        public int _turn;
        public float _userSuggest;
        public float _npcSuggest;
        public string _npcReaction;
    }

    GptResult _gptResult;

    public override void Enter()
    {
        SubScribeAction();
        
        _sendChatType = SendChatType.ChatBargain;
        //ServerManager.Instance.GetGPTReply("$start", _sendChatType);
        VariableList.S_GptAnswer = "reaction : 말씀은 이해합니다만, 저도 졸업을 앞둔 학생이라 금전적인 여유가 없습니다. 정말 100달러 정도면 할 수 있을 것 같아요. 이 금액을 초과하면 정말 어렵습니다.\r\n" +
            "persuasion : -1\r\n" +
            "vendorSuggest : 180\r\n" +
            "yourSuggest : 100";

        Managers.Chat.ActivatePanel(_sendChatType);

        _gptResult._turn = TurnInit;
    }

    public override void Exit()
    {
        UnSubScribeAction();
        //TODO : send evaluation needed! and save evaluation
    }

    public void UserInput(string user_input)
    {
        string user_send = MakeAnswer(user_input);
        //ServerManager.Instance.GetGPTReply(user_send, _sendChatType);
        VariableList.S_GptAnswer = "reaction : 춥고 배고프고 졸려요. persuasion : +1, vendorSuggest: 90, yourSuggest:100";
    }

    public void GptOutput(string gpt_output)
    {
        UpdateSuggest(gpt_output);
        if (!CheckTurn())
        {
            Managers.Chat.UpdateTurn(0, _gptResult._npcSuggest, _gptResult._userSuggest);
        }

        Managers.Chat.UpdatePanel(_gptResult._npcReaction);

        if (!CheckTurn())
        {
            Managers.Chat.TransitionToState(SendChatType.Fail);
        }
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string priceOpinion = Managers.Chat.ratePrice(_gptResult._userSuggest);
        string user_template = user_send + $"\n vendor Suggest: {_gptResult._userSuggest}"
                                + $" npc Suggest: {_gptResult._npcSuggest} price Opinion: {priceOpinion}";
        return user_template;
    }

    private void UpdateSuggest(string gpt_output)
    {
        _gptResult._npcReaction = ExtractStringValue(gpt_output, "reaction");
        _gptResult._turn += (int)ExtractFloatValue(gpt_output, "persuasion");

        float npcSuggest = ExtractFloatValue(gpt_output, "@yourSuggest");
        _gptResult._npcSuggest = npcSuggest !=-1.37f? (float)Math.Round(npcSuggest, 3) : _gptResult._npcSuggest;
        float vendorSuggest = ExtractFloatValue(gpt_output, "@vendorSuggest");
        _gptResult._userSuggest = vendorSuggest != -1.37f ? (float)Math.Round(vendorSuggest, 3) : _gptResult._userSuggest;
    }

    private bool CheckTurn()
    {
        if (_gptResult._turn > 0 || (_gptResult._turn == 0 && _gptResult._userSuggest <= _gptResult._npcSuggest))
        {
            _gptResult._turn = Math.Max(_gptResult._turn - 1, 0);

            if (_gptResult._userSuggest <= _gptResult._npcSuggest)
            {
                UpdateAndActivate();
            }
            return true;
        }
        return false;
    }

    private void UpdateAndActivate()
    {
        Managers.Chat.UpdateTurn(_gptResult._turn, _gptResult._npcSuggest, _gptResult._userSuggest);
        VariableList.AddItemPriceSold(_gptResult._npcSuggest);
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    private void SubScribeAction()
    {
        VariableList.OnVariableUserUpdated -= UserInput;
        VariableList.OnVariableGptUpdated -= GptOutput;

        VariableList.OnVariableUserUpdated += UserInput;
        VariableList.OnVariableGptUpdated += GptOutput;
    }
    private void UnSubScribeAction()
    {
        VariableList.OnVariableUserUpdated -= UserInput;
        VariableList.OnVariableGptUpdated -= GptOutput;
    }

    private string ExtractStringValue(string input, string tag)
    {
        string pattern = $@"{tag}\s*:\s*(.*?)(,|$|\r|\n)";
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }
        return "";
    }
    private float ExtractFloatValue(string input, string tag)
    {
        string pattern = $@"{tag}\s*:\s*(-?\d+(\.\d+)?)";
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            return float.Parse(match.Groups[1].Value);
        }
        return -1.37f;
    }
}
