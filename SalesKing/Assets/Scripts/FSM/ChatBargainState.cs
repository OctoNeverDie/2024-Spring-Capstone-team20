using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;
using static Define;
using static IVariableChat;

public class ChatBargainState : ChatBaseState, IVariableChat
{
    const int TurnInit = 8;
    private bool lastChance =false;
    private struct GptResult
    {
        public int _turn;
        public string _npcReaction;
        public float _userSuggest;
        public float _npcSuggest;
    }

    GptResult _gptResult = new GptResult();

    public override void Enter()
    {
        SubScribeAction();

        _gptResult._turn = TurnInit;
        Debug.Log($"Init Turn : {_gptResult._turn}");

        _sendChatType = SendChatType.ChatBargain;
        ServerManager.Instance.GetGPTReply("$start", _sendChatType);
        
        //VariableList.S_GptAnswer = "reaction : 말씀은 이해합니다만, 저도 졸업을 앞둔 학생이라 금전적인 여유가 없습니다. 정말 100달러 정도면 할 수 있을 것 같아요. 이 금액을 초과하면 정말 어렵습니다.\r\n" +"persuasion : -1\r\n" +"vendorSuggest : 180\r\n" +"yourSuggest : 100";

        Managers.Chat.ActivatePanel(_sendChatType);
        //여기있으면 왜 훨씬 나중에 실행되지?
    }

    public override void Exit()
    {
        UnSubScribeAction();
    }

    public void UserInput(string user_input)
    {
        string user_send;
        user_send = MakeAnswer(user_input);
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
        //VariableList.S_GptAnswer = "reaction : 춥고 배고프고 졸려요. persuasion : +1, vendorSuggest: 90, yourSuggest:100";
    }

    public void GptOutput(string gpt_output)
    {
        ConcatReply(gpt_output);

        bool isTurnLeft = CheckTurn();
        if (!isTurnLeft)
        {
            Managers.Chat.UpdateTurn(0, _gptResult._npcSuggest, _gptResult._userSuggest);
        }

        Managers.Chat.UpdatePanel(_gptResult._npcReaction);

        if (!isTurnLeft)
        {
            Debug.Log($"CheckTrun 이후 : {_gptResult._turn}");
            Managers.Chat.TransitionToState(SendChatType.Fail);
        }
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string priceOpinion = Managers.Chat.ratePrice(_gptResult._userSuggest);
        string user_template = user_send + $"\n vendor Suggest: {_gptResult._userSuggest}"
                                + $" npc Suggest: {_gptResult._npcSuggest} price Opinion: {priceOpinion}";
        if (lastChance)
            user_template = "$" + user_template;

        return user_template;
    }

    private void ConcatReply(string gptAnswer)
    {
        string[] sections = gptAnswer.Split(new string[] { "reaction", "vendorSuggest", "yourSuggest", "persuasion" }, StringSplitOptions.None);

        if (sections.Length > 4)
        {
            _gptResult._npcReaction = sections[1];
            _gptResult._userSuggest = GetFloat(sections[2]);
            _gptResult._npcSuggest = GetFloat(sections[3]);
            Debug.Log($"persu 빼기 전 : {_gptResult._turn}");
            _gptResult._turn += (int)GetFloat(sections[4]);
            Debug.Log($"persu 뺌: {_gptResult._turn}");
        }
        else if (sections.Length > 1)
        { _gptResult._npcReaction = sections[1]; }

        Debug.Log($"{_gptResult._npcReaction} + {_gptResult._turn} + {_gptResult._npcSuggest} + {_gptResult._userSuggest}");
    }

    private float GetFloat(string text)
    {
        Match match = Regex.Match(text, @"-?\d+(\.\d+)?");
        if (match.Success)
        {
            return float.Parse(match.Value, CultureInfo.InvariantCulture);
        }
        return -1.37f;

    }

    private bool CheckTurn()
    {
        //persuasion 해도 턴이 남았을 때
        if (_gptResult._turn >=2)
        {
            _gptResult._turn = Math.Max(_gptResult._turn - 1, 0);//턴 하나 빼기

            if (_gptResult._turn == 1)//마지막 찬스
            {
                lastChance = true;
            }
            if (_gptResult._userSuggest <= _gptResult._npcSuggest)
            {
                UpdateAndActivate();
            }
            return true;
        }

        //persuation으로 턴이 0이 됐을 때
        

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
}
