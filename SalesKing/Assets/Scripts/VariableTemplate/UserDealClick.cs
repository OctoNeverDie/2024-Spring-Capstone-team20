using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UserDealClick : MonoBehaviour
{
    public void OnClickYes()
    {
        ServerManager.Instance.GetGPTReply("$buy", SendChatType.Success);
        //Managers.Chat.TestReply("Success");
        this.gameObject.SetActive(false);
    }


}
