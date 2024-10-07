using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNoClick : MonoBehaviour
{
    public void OnClick()
    {
        Managers.Chat.CheckTurnEndpoint(Define.EndType.Leave);
    }
}
