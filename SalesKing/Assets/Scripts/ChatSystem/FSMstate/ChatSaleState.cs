using System;
using UnityEngine;
using UnityEngine.Windows;
using static Define;
using static IVariableChat;

public class ChatSaleState : ChatBaseState, IVariableChat
{
    private struct GptResult
    {
        public string _reaction;
        public string _thingToBuy;
        public bool _isYesNo;//yes나 no가 있나?
        public bool _yesIsTrue;//있다면, yes인가?
        public string _evaluation;
    }

    GptResult _gptResult = new GptResult();
    bool isEndHere = false;
    public override void Enter()
    {
        _sendChatType = SendChatType.ChatSale;
        //맨 처음 시작할 때, convo ui 나와야한다.
        Managers.Chat.ActivatePanel(_sendChatType);
        SubScribeAction();
    }

    public override void Exit()
    {
        UnSubScribeAction();
        _gptResult = new GptResult();
        //save evaluation

        if (isEndHere)
        {
            isEndHere = false;
            Managers.Chat._endType = EndType.Fail;
            ServerManager.Instance.GetGPTReply("$clear", SendChatType.Endpoint);
        }
    }

    //유저가 입력할 때 : 
    public void UserInput(string type, string user_input)
    {//그대로 보낸다
        if (type != nameof(Managers.Chat.ReplyManager.UserAnswer))
            return;
        Debug.Log($"ChatSaleState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    //GPT 답 돌아왔을 때: 
    public void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;
        CheckYesOrNo(gpt_output);//yes,no 왔는지 체크
        ConcatReply(gpt_output);//gpt 답변 _gptResult에 업데이트

        Debug.Log($"지워 :_gptResult._thingToBuy {_gptResult._thingToBuy}+_gptResult._reaction{_gptResult._reaction}+_gptResult._evaluation{_gptResult._evaluation}");
        Debug.Log($"지워: _gptResult._yesOrNo {_gptResult._isYesNo} _gptResult._yesIsTrue {_gptResult._yesIsTrue}");
        CheckChangeState();//다음 행동
    }

    private void CheckChangeState()
    {
        if (!_gptResult._isYesNo)
        {
            return;
        }
        Debug.Log("ChatSaleState");
        Managers.Chat.EvalManager.AddEvaluation(_gptResult._evaluation);
        Managers.Chat.EvalManager.ThingToBuy = _gptResult._thingToBuy;

        if (_gptResult._yesIsTrue)
        {
            Managers.Chat.TransitionToState(SendChatType.ItemInit);
        }
        else if (!_gptResult._yesIsTrue)
        {
            isEndHere = true;
            Exit();
        }
    }
    private void CheckYesOrNo(string gptAnswer)
    {
        _gptResult._isYesNo = false;
        _gptResult._yesIsTrue = false;

        string[] markers = { "yes", "no" };
        
        foreach (string marker in markers)
        {
            int index = gptAnswer.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                _gptResult._isYesNo = true;
                _gptResult._yesIsTrue = marker.Equals("yes", StringComparison.OrdinalIgnoreCase);
                break;
            }
        }
    }

    private void ConcatReply(string gptAnswer)
    {
        string[] sections = gptAnswer.Split(new string[] { "ThingToBuy", "yourReply", "summary" }, StringSplitOptions.None);

        if (sections.Length > 3)//yes일 때,
        {
            _gptResult._thingToBuy = sections[1];
            _gptResult._reaction = sections[2];
            _gptResult._evaluation = sections[3];
        }

        else if (sections.Length > 2)//no일 때,
        {
            _gptResult._reaction = sections[1];
            _gptResult._evaluation = sections[2];
        }

        else if (sections.Length > 1)
        {
            _gptResult._reaction = sections[1];//later : Trim()

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
