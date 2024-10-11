using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FSMBtn : MonoBehaviour
{
    //말을 건다 버튼 누르면
    public void OnClickYesTalkFSM()
    {
        //start fsm
        Managers.Chat.Init();
    }

    //떠나기 버튼 누르면
    public void OnLeaveClickFSM()
    {
        Managers.Chat._endType = EndType.Fail;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }

    //Deal 버튼 누르면
    public void OnClickDealFSM()
    {
        Managers.Chat.EvalManager.AddItemPriceSold();
        ServerManager.Instance.GetGPTReply("$buy", SendChatType.Endpoint);
        //go to template recieve
        this.gameObject.SetActive(false);
    }
}
