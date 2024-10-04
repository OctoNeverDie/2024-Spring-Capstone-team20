using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDealClick : MonoBehaviour
{
    public void OnClickYes()
    {
        ChatManager.ChatInstance.TestReply("Success");
        this.gameObject.SetActive(false);
    }

    public void OnClickNo()
    {
        ChatManager.ChatInstance.CheckTurnFail();
        this.gameObject.SetActive(false);
    }
}
