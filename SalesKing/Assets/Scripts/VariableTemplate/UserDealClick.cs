using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDealClick : MonoBehaviour
{
    public void OnClickYes()
    {
        Managers.Chat.TestReply("Success");
        this.gameObject.SetActive(false);
    }

    public void OnClickNo()
    {
        Managers.Chat.CheckTurnFail();
        this.gameObject.SetActive(false);
    }
}
