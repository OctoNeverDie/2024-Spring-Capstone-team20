using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessState : ChatBaseState
{
    public override void Enter()
    {
        _sendChatType = Define.SendChatType.Success;

        Managers.Chat.ActivatePanel(_sendChatType);
        //TODO : variableList.currentNPC의 itemPrice에 합의요금 업데이트
        //유저의 bill system에 variableList.itemPrice에 업데이트
        ClearVariables();
    }

    private void ClearVariables()
    {
        //VariableList.Clear(); 만들기.
    }
}
