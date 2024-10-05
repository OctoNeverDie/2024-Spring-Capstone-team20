using System;
using UnityEngine;
using static Define;

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

    GptResult _gptResult;

    public override void Enter()
    {
        Managers.Chat.ActivatePanel(SendChatType.ChatSale);
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
        //ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
        VariableList.S_GptAnswer = "아, 저 살래요! @yes @ThingToBuy: (상인이 네게 제안한 물건) @Summary: 왜 이 물건을 사려고 하는지. 상인에 대해 어떤 감정을 느끼는지에 대한 서술.";
    }

    public void GptOutput(string gpt_output)
    {
        _gptResult = CheckYesOrNo(gpt_output);
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
            Managers.Chat.TestReply("ItemInit");
        }
        else if (!_gptResult._yesIsTrue)
        {
            Managers.Chat.TestReply("Fail");
        }
    }
    private GptResult CheckYesOrNo(string gptAnswer)
    {
        GptResult result = new GptResult();
        result._yesOrNo = null;
        result._yesIsTrue = false;

        string[] markers = { "@yes", "@no" };
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
                result._yesOrNo = gptAnswer.Substring(index, marker.Length);
                result._yesIsTrue = marker.Equals("@yes", StringComparison.OrdinalIgnoreCase);
                break;
            }
        }

        if (result._yesOrNo != null)
        {
            string[] splitParts = gptAnswer.Split(new string[] { result._yesOrNo }, StringSplitOptions.None);
            result._reaction = splitParts[0].Trim();

            if (splitParts.Length > 1)
            {
                string remainingText = splitParts[1].Trim();
                string[] sections = remainingText.Split(new string[] { "@ThingToBuy:", "@Summary:" }, StringSplitOptions.None);
                
                if (sections.Length > 1)
                {
                    result._thingToBuy = sections[1].Trim();
                }

                if (sections.Length > 2)
                {
                    result._evaluation = sections[2].Trim();
                }
            }
        }
        else
        { 
            result._reaction = gptAnswer;
        }
        
        return result;
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
