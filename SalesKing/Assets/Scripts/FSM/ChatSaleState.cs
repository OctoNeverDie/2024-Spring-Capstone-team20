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
        Managers.Chat.ActivatePanel(_sendChatType);
        SubScribeAction();
    }

    public override void Exit()
    {
        UnSubScribeAction();
        _gptResult = new GptResult();
        //save evaluation
        VariableList.AddEvaluation(_gptResult._evaluation);
        VariableList.S_ThingToBuy = _gptResult._thingToBuy;

        if (isEndHere)
        {
            isEndHere = false;
            ServerManager.Instance.GetGPTReply("$clear", SendChatType.Endpoint);
            Managers.Chat.CheckTurnEndpoint(Define.EndType.Fail);
            Managers.Chat.ActivatePanel(SendChatType.Endpoint);
            Managers.Chat.Clear();
        }
    }

    //유저가 입력할 때 : 
    public void UserInput(string user_input)
    {//그대로 보낸다
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
    }

    //GPT 답 돌아왔을 때: 
    public void GptOutput(string gpt_output)
    {
        CheckYesOrNo(gpt_output);//yes,no 왔는지 체크
        ConcatReply(gpt_output);//gpt 답변 _gptResult에 업데이트

        Managers.Chat.UpdatePanel(_gptResult._reaction);//gpt reaction
        
        CheckChangeState();//다음 행동
    }

    private void CheckChangeState()
    {
        if (!_gptResult._isYesNo)
        {
            return;
        }
        else if (_gptResult._yesIsTrue)
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

        Debug.Log($"지워 : gptAnswer {gptAnswer}");
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
        Debug.Log($"지워: _gptResult._yesOrNo {_gptResult._isYesNo} _gptResult._yesIsTrue {_gptResult._yesIsTrue}");
    }

    private void ConcatReply(string gptAnswer)
    {
        string[] sections = gptAnswer.Split(new string[] { "ThingToBuy", "yourReply", "summary" }, StringSplitOptions.None);

        if (sections.Length > 3)
        {
            _gptResult._thingToBuy = sections[1];
            _gptResult._reaction = sections[2];
            _gptResult._evaluation = sections[3];
            Managers.Chat.UpdateThingToBuy(_gptResult._thingToBuy);
            Debug.Log($"지워 :_gptResult._thingToBuy {_gptResult._thingToBuy}+_gptResult._reaction{_gptResult._reaction}+_gptResult._evaluation{_gptResult._evaluation}");
        }

        else if (sections.Length > 1)
        {
            Debug.Log($"지워 : _gptResult._reaction {sections[1]}");
            _gptResult._reaction = sections[1];//later : Trim()
            
        }
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
