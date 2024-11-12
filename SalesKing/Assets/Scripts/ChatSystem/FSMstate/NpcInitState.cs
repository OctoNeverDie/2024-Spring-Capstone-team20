using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 말을 건다 버튼 클릭하면 일어남.
/// </summary>
public class NpcInitState : ChatBaseState
{
    private NpcInfo npc;
    private ItemInfo playerItem;

    public override void Enter()
    {   
        ChatManager.Instance.Clear();//data, front 모두

        npc = ChatManager.Instance.ThisNpc;
        playerItem = GetRandItem();

        ShowFront();
        SendBack();
    }
    public override void Exit()
    {
        SaveData();
    }
    private void SaveData()
    {
        ChatManager.Instance.Eval.InitEvalDictNpc(npc.NpcID, playerItem.ObjID);
    }

    private void ShowFront()
    {
        //show npc : npc name
        //show tablet : npc name ~ npc want item
        //npc item 룰렛 : 돌리기 until item 정해지기까지 : 2f
        //show item random
    }

    private void SendBack()
    {
        _sendChatType = SendChatType.ChatInit;

        string _userSend = MakeUserSend(npc);
        string[] _mbtis = MakeMbtiSend(npc.Mbtis);

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}. {_userSend}, {_mbtis}");
        ServerManager.Instance.GetGPTReply("$start", _sendChatType, _userSend, _mbtis);
    }

    private ItemInfo GetRandItem()
    {
        List<ItemInfo> categorizedList= DataGetter.Instance.CategorizedItems[npc.ItemCategory];
        int randomIdx = Random.Range(0, categorizedList.Count);
        return categorizedList[randomIdx];
    }


    private string[] MakeMbtiSend(int[] mbtiPrefers)
    {
        string[] strMbti = new string[4];
        for (int i = 0; i < mbtiPrefers.Length; i++)
        {
            if (mbtiPrefers[i] == 1)
                strMbti[i] = "like";
            else if (mbtiPrefers[i] == 0)
                strMbti[i] = "norm";
            else if (mbtiPrefers[i] == -1)
                strMbti[i] = "dislike";
        }
        return strMbti;
    }

    private string MakeUserSend(NpcInfo npc)
    {
        string user_send = $"\n NpcName : {npc.NpcName}, NpcSex : {npc.NpcSex}, NpcAge : {npc.NpcAge} "
            + $" KeyWord : {npc.KeyWord}, \nPersonailty : {npc.Personality}\nDialogue Style: {npc.DialogueStyle}\nExample: {npc.Example}"
            +$"Situation : {npc.SituationDescription}"
            + $"당근에 올린 글: {npc.Concern}\n네가 사려고 한 물건: {npc.WantItem}, 판매자가 가져온 물건: {playerItem.ObjName} ";

        return user_send;
    }
}
