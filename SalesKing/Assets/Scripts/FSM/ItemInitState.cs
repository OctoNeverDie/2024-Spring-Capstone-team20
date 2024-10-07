using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemInitState : ChatBaseState
{
    string _userSend;
    public override void Enter()
    {
        VariableList.OnItemInit -= MakeAnswer;
        VariableList.OnItemInit += MakeAnswer;

        _sendChatType = SendChatType.ItemInit;
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    public override void Exit()
    {
        VariableList.OnItemInit -= MakeAnswer;
    }
    private void MakeAnswer(float userSuggest, ItemInfo itemInfo)
    {
        /* some format to send
         * public class ItemInfo
        {
            public int ObjID;
            public string ObjName;
            public string ObjInfo;
            public int npcFirstSuggestPrice;;
            public int expensive;
            public int tooExpensive;
        }
         * The thing you want to buy: 동기부여 관련 책
        The thing vendor is selling to you:  책
        vendor First Suggest: 200$, Your First Suggest: 50$, yourOpinion: too expensive
         */
        string expensiveRate = Managers.Chat.ratePrice(userSuggest, itemInfo);

        _userSend = $"\nThe thing you want to buy: {VariableList.S_ThingToBuy}"
        + $"\nThe thing vendor is selling to you: {itemInfo.ObjName}"
        + $"\nvendor First Suggest: {userSuggest} credit,"
        + $"Your First Suggest: {itemInfo.defaultPrice} credit"
        + $"yourOpinion: {expensiveRate}";

        ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
        //Managers.Chat.TestReply("ChatBargain", _userSend);
    }
}
