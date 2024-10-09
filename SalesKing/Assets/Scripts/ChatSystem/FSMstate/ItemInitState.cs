using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemInitState : ChatBaseState
{
    string _userSend;
    public override void Enter()
    {
        EvalSubManager.OnItemInit -= MakeAnswer;
        EvalSubManager.OnItemInit += MakeAnswer;

        _sendChatType = SendChatType.ItemInit;
        Debug.Log($"ItemInitState에서 보냄1 {_sendChatType}");
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    public override void Exit()
    {
        EvalSubManager.OnItemInit -= MakeAnswer;
    }
    private void MakeAnswer(float userSuggest, ItemInfo itemInfo)
    {
        string expensiveRate = Managers.Chat.RatePrice(userSuggest, itemInfo);

        _userSend = $"\nThe thing you want to buy: {Managers.Chat.EvalManager.ThingToBuy}"
        + $"\nThe thing vendor is selling to you: {itemInfo.ObjName}"
        + $"\nvendor First Suggest: {userSuggest} credit,"
        + $"Your First Suggest: {itemInfo.defaultPrice} credit"
        + $"yourOpinion: {expensiveRate}";

        Debug.Log($"ItemInitState에서 보냄2 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }
}
