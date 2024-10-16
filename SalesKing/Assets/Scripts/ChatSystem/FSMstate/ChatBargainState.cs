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
        Success,
        Fail
    }
    private struct GptResult
    {
        public int _turn;
        public string _npcReaction;
        public float _userSuggest;
        public float _npcSuggest;
        public string _summary;
    }

    GptResult _gptResult = new GptResult();
    State isState = State.Wait;

    public override void Enter()
    {
        SubScribeAction();

        _gptResult._turn = TurnInit;
        _sendChatType = SendChatType.ChatBargain;

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

        if (isState != State.Wait)
            CheckState();

        UpdateTurn();
        ReactToState();
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string priceOpinion = Managers.Chat.RatePrice(_gptResult._userSuggest);
        string user_template = user_send + $"\n vendor Suggest: {_gptResult._userSuggest}"
                                + $" npc Suggest: {_gptResult._npcSuggest}\n price Opinion: {priceOpinion}\nturn : {_gptResult._turn}";

        return user_template;
    }

    private void CheckState()
    {
        if (_gptResult._userSuggest!=0 && _gptResult._userSuggest <= _gptResult._npcSuggest)
        {
            StateFailSuccess(State.Success, 3, EndType.buy);
        }
        //persuasion 해도 턴이 남았을 때
        if (_gptResult._turn >=1)
        {
            //받고, 업데이트 했으니 이제 플레이어가 보내야하는 상황의 턴이 나옴
            _gptResult._turn = Math.Max(_gptResult._turn - 1, 0);//턴 하나 빼기
            StateFailSuccess(State.Wait, 0, EndType.None);
        }
        else //(_gptResult._turn <=0)
        {
            StateFailSuccess(State.Fail, 2, EndType.reject);
        }
    }

    private void ReactToState()
    {
        if (isState == State.Success)
        {
            float smaller = (_gptResult._npcSuggest > _gptResult._userSuggest) ? _gptResult._userSuggest : _gptResult._npcSuggest;
            Managers.Chat.EvalManager.UpdateSuggestInEval(smaller);
            Managers.Chat.EvalManager.AddItemPriceSold();
        }

        if (isState != State.Wait)
        {
            Managers.Chat.TransitionToState(SendChatType.Endpoint);
        }
    }

    private void UpdateTurn()
    {
        Managers.Chat.UpdateTurn(_gptResult._turn, _gptResult._npcSuggest, _gptResult._userSuggest);
    }

    private void ConcatReply(string gptAnswer)
    {
        if (gptAnswer.Contains("summary"))////npc가 끝냄
        {
            NpcEnds(gptAnswer);
            return;
        }

        //일반
        string[] sections = gptAnswer.Split(new string[] { "reaction", "vendorSuggest", "customerSuggest", "persuasion" }, StringSplitOptions.None);

        if (sections.Length > 4)
        {
            _gptResult._npcReaction = sections[1];
            _gptResult._userSuggest = GetFloat(sections[2]);
            _gptResult._npcSuggest = GetFloat(sections[3]);
            _gptResult._turn += (int)GetFloat(sections[4]);
        }
        else if (sections.Length > 1)//맨 처음
        {
            _gptResult._npcReaction = sections[1];
            _gptResult._npcSuggest = -1f;//not yet
        }
    }

    private void NpcEnds(string gptAnswer)
    {
        string[] sections2 = gptAnswer.Split(new string[] { "summary", "action", "finalPrice" }, StringSplitOptions.None);
        _gptResult._summary = sections2[1].Trim();
        Managers.Chat.EvalManager.AddEvaluation(_gptResult._summary);

        string actionValue = sections2[2].Trim();
        if (actionValue == "bought")
        {
            Debug.Log("들어옴 bout1");
            float finalPrice = GetFloat(sections2[3]);
            _gptResult._npcSuggest = finalPrice;
            _gptResult._userSuggest = finalPrice;
            StateFailSuccess(State.Success, 3, EndType.clear);
        }
        else if (actionValue == "notBought")
        {
            Debug.Log("들어옴 not bout1");
            StateFailSuccess(State.Fail, 1, EndType.clear);
        }
    }

    private void StateFailSuccess(State state, int reason, EndType endType)
    {
        isState = state;
        Managers.Chat.reason = reason;
        Managers.Chat._endType = endType;
    }

    private float GetFloat(string text)
    {
        Match match = Regex.Match(text, @"-?\d+(\.\d+)?");
        if (match.Success)
        {
            return float.Parse(match.Value, CultureInfo.InvariantCulture);
        }
        return -1f;

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