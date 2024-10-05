using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNoClick : MonoBehaviour
{
    public void OnClick()
    {
        Managers.Chat.TestReply("Leave");
    }
}
