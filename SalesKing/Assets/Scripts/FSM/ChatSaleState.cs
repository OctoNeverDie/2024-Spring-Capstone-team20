using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ChatSaleState : ChatBaseState, IVariableChat
{
    public struct GptResult
    {
        public string _reaction;
        public string _yesOrNo;
        public bool _yesIsTrue;
        public string _evaluation;
    }

    GptResult _gptResult;

    public override void Enter()
    {
        VariableList.OnVariableUserUpdated += UserInput;
        VariableList.OnVariableGptUpdated += GptOutput;
    }

    public override void Exit()
    {
        //save evaluation
        VariableList.AddEvaluation(_gptResult._evaluation);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckChangeState();
        }
    }

    public void UserInput(string user_input)
    {
        VariableList.S_GptAnswer = "아, 저 살래요! @yes 평가내용 좔좔좔";
        //ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }

    public void GptOutput(string gpt_output)
    {
        _gptResult = CheckYesOrNo(gpt_output);
        //Show reaction to User
        //Update Log
        Debug.Log($"reaction : {_gptResult._reaction}, evaluation : {_gptResult._evaluation}");
    }

    private void CheckChangeState()
    {
        if (_gptResult._yesOrNo == null)
        {
            return;
        }
        else if (_gptResult._yesIsTrue)
        {
            ChatManager.ChatInstance.TestReply("ItemInit");
        }
        else if (!_gptResult._yesIsTrue)
        {
            ChatManager.ChatInstance.TestReply("NpcNo");
        }
    }
    private GptResult CheckYesOrNo(string gptAnswer)
    {
        GptResult result = new GptResult();
        result._yesOrNo = null;
        result._yesIsTrue = false;

        string[] markers = { "@yes", "@no" };

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
            result._evaluation = splitParts.Length > 1 ? splitParts[1].Trim() : "";
        }
        else
        { 
            result._reaction = gptAnswer;
        }
        
        return result;
    }
}
