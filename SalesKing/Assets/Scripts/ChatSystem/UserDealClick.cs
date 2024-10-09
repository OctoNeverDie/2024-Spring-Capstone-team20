using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UserDealClick : MonoBehaviour
{
    //Deal 버튼 누르면
    public void OnClickYesFSM()
    {
        ServerManager.Instance.GetGPTReply("$buy", SendChatType.Endpoint);
        this.gameObject.SetActive(false);
    }

    //endpanel 확인버튼 누르면
    public void OnEndChatFSM()
    {
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }
}
