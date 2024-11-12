using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class City_ChattingAction : MonoBehaviour
{
    public void OnClickYesTalkFSM()
    {
        //start fsm
        UIManager.Chat.Init();
    }

    //떠나기 버튼 누르면
    public void OnClickLeaveFSM()
    {
        Managers.Chat._endType = EndType.leave;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }

    //최종 버튼 누르면
    public void OnClickFinalFSM()
    {
        Managers.Turn.AddTurnAndCheckTalkTurn();
    }
}
