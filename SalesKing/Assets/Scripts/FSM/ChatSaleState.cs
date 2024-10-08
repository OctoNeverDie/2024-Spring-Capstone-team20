using System;
using UnityEngine;
using static Define;
using static IVariableChat;

public class ChatSaleState : ChatBaseState, IVariableChat
{
    private struct GptResult
    {
        public string _reaction;
        public string _thingToBuy;
        public string _yesOrNo;
        public bool _yesIsTrue;
        public string _evaluation;
    }

    GptResult _gptResult = new GptResult();

    public override void Enter()
    {
        _sendChatType = SendChatType.ChatSale;
        Managers.Chat.ActivatePanel(_sendChatType);
        SubScribeAction();
    }

    public override void Exit()
    {
        UnSubScribeAction();
        //save evaluation
        VariableList.AddEvaluation(_gptResult._evaluation);
        VariableList.S_ThingToBuy = _gptResult._thingToBuy;
    }

    public void UserInput(string user_input)
    {
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
        //VariableList.S_GptAnswer = "아, 저 살래요! @yes @ThingToBuy: (상인이 네게 제안한 물건) @Summary: 왜 이 물건을 사려고 하는지. 상인에 대해 어떤 감정을 느끼는지에 대한 서술.";
    }

    public void GptOutput(string gpt_output)
    {
        CheckYesOrNo(gpt_output);
        ConcatReply(gpt_output);

        Managers.Chat.UpdatePanel(_gptResult._reaction);
        //Show reaction to User
        //Update Log
        Debug.Log($"reaction : {_gptResult._reaction}, evaluation : {_gptResult._evaluation}");
        CheckChangeState();
    }

    private void CheckChangeState()
    {
        if (_gptResult._yesOrNo == null)
        {
            return;
        }
        else if (_gptResult._yesIsTrue)
        {
            Managers.Chat.TransitionToState(SendChatType.ItemInit);
            //Managers.Chat.TestReply("ItemInit");
        }
        else if (!_gptResult._yesIsTrue)
        {
            Managers.Chat.TransitionToState(SendChatType.Endpoint);
            //Managers.Chat.TestReply("Fail");
        }
    }
    private void CheckYesOrNo(string gptAnswer)
    {
        _gptResult._yesOrNo = null;
        _gptResult._yesIsTrue = false;

        string[] markers = { "yes", "no" };
        /*        
        @yes
        @ThingToBuy: (상인이 네게 제안한 물건)
        @Summary: 왜 이 물건을 사려고 하는지. 상인에 대해 어떤 감정을 느끼는지에 대한 서술.
         */
        foreach (string marker in markers)
        {
            int index = gptAnswer.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                _gptResult._yesOrNo = gptAnswer.Substring(index, marker.Length);
                _gptResult._yesIsTrue = marker.Equals("yes", StringComparison.OrdinalIgnoreCase);
                break;
            }
        }
    }

    private void ConcatReply(string gptAnswer)
    {
        string[] sections = gptAnswer.Split(new string[] { "ThingToBuy", "yourReply", "summary" }, StringSplitOptions.None);

        if (sections.Length > 3)
        {
            _gptResult._thingToBuy = sections[1];
            _gptResult._reaction = sections[2];
            _gptResult._evaluation = sections[3];
            Debug.Log($"{_gptResult._thingToBuy}+{_gptResult._reaction}+{_gptResult._evaluation}");
        }

        else if (sections.Length > 1)
        {
            Debug.Log($"reply : {sections[1]}");
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
