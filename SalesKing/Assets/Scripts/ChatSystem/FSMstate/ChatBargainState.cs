using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public class ChatBargainState : ChatBaseState, IVariableChat
{
    const int TurnInit = 8;    

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
    State isState = State.Wait;

    public override void Enter()
    {
        SubScribeAction();

        _gptResult._turn = TurnInit;

        _sendChatType = SendChatType.ChatBargain;
        ServerManager.Instance.GetGPTReply("$start", _sendChatType);
        
        //맨 처음 시작할 때, convo ui 나와야한다.
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    public override void Exit()
    {        
        UnSubScribeAction();
    }

    public void UserInput(string type, string user_input)
    {
        if (type != nameof(Managers.Chat.ReplyManager.UserAnswer))
            return;

        string user_send;
        user_send = MakeAnswer(user_input);

        Debug.Log($"ChatBargainState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    public void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        ConcatReply(gpt_output);

        if (isState != State.Fail)
            isState = CheckState();

        UpdateTurn();
        ReactToState();
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string priceOpinion = Managers.Chat.RatePrice(_gptResult._userSuggest);
        string user_template = user_send + $"\n vendor Suggest: {_gptResult._userSuggest}"
                                + $" npc Suggest: {_gptResult._npcSuggest} price Opinion: {priceOpinion}";

        return user_template;
    }

    private State CheckState()
    {
        if (_gptResult._userSuggest!=0 && _gptResult._userSuggest <= _gptResult._npcSuggest)
        {
            Managers.Chat.reason = 3;
            return State.Succes;
        }
        //persuasion 해도 턴이 남았을 때
        if (_gptResult._turn >=2)
        {
            //받고, 업데이트 했으니 이제 플레이어가 보내야하는 상황의 턴이 나옴
            _gptResult._turn = Math.Max(_gptResult._turn - 1, 0);//턴 하나 빼기
            return State.Wait;//다음 대화
        }
        _gptResult._turn = 0;
        Managers.Chat.ReplyManager.GptReaction = "음, 싫어요.";
        Managers.Chat.reason = 2;
        return State.Fail;
    }

    public static Action<bool, float> ChatBargainReactState;
    private void ReactToState()
    {
        Debug.Log($"ChatBargainState에서 보냄 {isState}");
        if (isState == State.Fail)
        {
            ChatBargainReactState?.Invoke(false, 0);
        }
        else if (isState == State.Succes)
        {
            float smaller = (_gptResult._npcSuggest > _gptResult._userSuggest) ? _gptResult._userSuggest : _gptResult._npcSuggest;
            ChatBargainReactState?.Invoke(true, smaller);
            //UpdateAndActivate();
        }
    }

    private void UpdateTurn()
    {
        Managers.Chat.UpdateTurn(_gptResult._turn, _gptResult._npcSuggest, _gptResult._userSuggest);
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
                isState = State.Fail ;
                _gptResult._turn = 0;
                Managers.Chat.reason = 1; //NPC 기분이 나빠서 Fail
                Managers.Chat.ReplyManager.GptReaction = "음, 싫어요.";
                return;
            }

            _gptResult._turn += (int)GetFloat(sections[4]);
        }
        else if (sections.Length > 1)//맨 처음
        {
            _gptResult._npcReaction = sections[1];
            _gptResult._npcSuggest = -1f;//not yet
        }
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