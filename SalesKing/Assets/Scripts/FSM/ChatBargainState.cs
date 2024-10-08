using System;
using System.Globalization;
using System.Runtime.CompilerServices;
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
    State isState = State.Wait;

    private enum State
    { 
        Wait,
        Succes,
        Fail
    }
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
        Debug.Log($"지워 : Init Turn : {_gptResult._turn}");

        _sendChatType = SendChatType.ChatBargain;
        ServerManager.Instance.GetGPTReply("$start", _sendChatType);
        
        //맨 처음 시작할 때, convo ui 나와야한다.
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    public override void Exit()
    {
        UpdateAndActivate();
        UnSubScribeAction();
    }

    public void UserInput(string user_input)
    {
        string user_send;
        user_send = MakeAnswer(user_input);

        Debug.Log($"ChatBargainState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    public void GptOutput(string gpt_output)
    {
        ConcatReply(gpt_output);

        if(isState != State.Fail)
            isState = CheckState();

        Debug.Log($"ChatBargainState에서 보냄 {isState.ToString()}");
        if (isState == State.Fail)
        {
            UpdateTurn();
            Managers.Chat._endType = EndType.Fail;
            Managers.Chat.TransitionToState(SendChatType.Endpoint);
        }
        else if (isState == State.Wait)
        {
            UpdateTurn();
            return;
        }
        else if (isState == State.Succes)
        {
            UpdateAndActivate();
            Managers.Chat._endType = EndType.Success;
            Managers.Chat.TransitionToState(SendChatType.Endpoint);
        }
        
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string priceOpinion = Managers.Chat.RatePrice(_gptResult._userSuggest);
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
            
            int persuasion = (int)GetFloat(sections[4]);

            if (persuasion <= -20)
            {
                isState = State.Fail;
                return;
            }

            _gptResult._turn += (int)GetFloat(sections[4]);
            
        }
        else if (sections.Length > 1)
        { _gptResult._npcReaction = sections[1]; }
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

    private State CheckState()
    {
        //persuasion 해도 턴이 남았을 때
        if (_gptResult._turn >=1)
        {
            //받고, 업데이트 했으니 이제 플레이어가 보내야하는 상황의 턴이 나옴
            _gptResult._turn = Math.Max(_gptResult._turn - 1, 0);//턴 하나 빼기

            if (_gptResult._turn == 1)
            {
                lastChance = true;
            }

            return State.Wait;//다음 대화
        }

        //만약 마지막 턴이었을 때(persuasion 무시됨)vendor랑 npc랑 비교
        if (lastChance && (_gptResult._userSuggest <= _gptResult._npcSuggest))
        {
            //성공
            lastChance = false;
            return State.Succes;
        }

        return State.Fail;
    }

    private void UpdateAndActivate()//마지막으로 갱신됨
    {
        UpdateTurn();
        //최종값 올림
        VariableList.AddItemPriceSold(_gptResult._npcSuggest);
    }

    private void UpdateTurn()
    {
        Managers.Chat.UpdateTurn(_gptResult._turn, _gptResult._npcSuggest, _gptResult._userSuggest);
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
