using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemInitState : ChatBaseState
{
    public override void Enter()
    {
        
        
        _sendChatType = SendChatType.NpcInit;
        ChatManager.ChatInstance.ActivatePanel(_sendChatType);
        //Inventory panel 나옴
        //panel 중 item 선택, iteminfo로 받겠지 : 
        //input field에서 first suggest도 받겠지.
        /*
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

    }

    private void ItemInit(float userSuggest, ItemInfo itemInfo)
    { 

    }

    protected override string MakeAnswer(string user_send = "")
    { 
        return 
    }
}
