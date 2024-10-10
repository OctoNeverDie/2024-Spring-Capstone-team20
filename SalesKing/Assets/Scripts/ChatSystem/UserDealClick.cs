using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UserDealClick : MonoBehaviour
{
    //Deal 버튼 누르면
    public void OnClickYesFSM()
    {
        Managers.Chat.EvalManager.AddItemPriceSold();
        ServerManager.Instance.GetGPTReply("$buy", SendChatType.Endpoint);
        //go to template recieve
        this.gameObject.SetActive(false);
    }

    //떠나기 버튼 누르면
    public void OnLeaveClickFSM()
    {
        Managers.Chat._endType = EndType.Leave;
        OnEndChatFSM();
    }
    //endpanel 확인버튼 누르면
    public void OnEndChatFSM()
    {
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }
}
