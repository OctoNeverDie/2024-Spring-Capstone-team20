using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

/// <summary>
/// 말을 건다 버튼 클릭하면 일어남.
/// </summary>
public class NpcInitState : ChatBaseState
{
    private NpcInfo npc;

    public override void Enter()
    {
        _sendChatType = SendChatType.ChatInit;
        npc = Chat.ThisNpc;

        ShowFront();
    }
    public override void Exit()
    {
        SendBack();
    }

    private void ShowFront()
    {
        Chat.ActivatePanel(_sendChatType, npc.RealItem);
    }

    private void SendBack()
    {
        if(npc==null)
            npc = Chat.ThisNpc;

        if (npc.NpcID == 0)
        {
            Chat.FirstNpc();
            return;
        }
        
        string _userSend = MakeUserSend(npc) + "\n" + MakeMbtiSend(npc.Mbtis);

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}. {_userSend}");
        ServerManager.Instance.GetGPTReply("$start", _sendChatType, _userSend);
    }

    private string MakeMbtiSend(int[] mbtiPrefers)
    {
        //        기본좋아, 구걸좋아, 관계형성좋아, 아부좋아
        string[] strMbtis = new string[] { "#구걸", "#기본", "#아부", "#관계형성" };
        string result = "";
        for (int i = 0; i < mbtiPrefers.Length; i++)
        {
            string strMbti = strMbtis[i];
            string preference = "";

            switch (mbtiPrefers[i])
            {
                case -1:
                    preference = " 싫어(persuasion: -1) ";
                    break;
                case 0:
                    preference = " 보통(persuasion: -1to+1)";
                    break;
                case 1:
                    preference = " 좋아(persuasion: +1)";
                    break;
            }

            result += strMbti + preference;
        }

        return result;
    }

    private string MakeUserSend(NpcInfo npc)
    {
        string user_send = $"\"NpcName\" : \"{npc.NpcName}\", \"NpcSex\" : \"{npc.NpcSex}\", \"NpcAge\" : {npc.NpcAge} "
            + $"\"KeyWord\" : \"{npc.KeyWord}\", \n\"Personailty\" : \"{npc.Personality}\"\nDialogue Style: {npc.DialogueStyle}\nExample: {npc.Example}\n"+"}"
            + $"\nConcern: {npc.Concern} \n wantItem= {npc.WantItem}\n RealItem(user의 말에 따라 인식이 바뀔 수 있음)= {npc.RealItem}\n";

        user_send += AddPrevNews(npc.NpcID);

        return user_send;
    }

    private string AddPrevNews(int npcID) {
        if (npc.NpcID != 3)//제리체리제리가 아니라면
            return "";
        return Chat.FindEval(npc.NpcID);
    }


    private ItemInfo GetRandItem()
    {
        int randomIdx;
        ItemInfo thisItem;

        if (npc.ItemCategory == ItemCategory.Random)
        {
            var items = DataGetter.Instance.ItemList;
            randomIdx = Random.Range(0, items.Count);

            thisItem = items[randomIdx];
        }
        else
        {
            List<ItemInfo> categorizedList = DataGetter.Instance.CategorizedItems[npc.ItemCategory];

            if (categorizedList.Count > 0)
            {
                randomIdx = Random.Range(0, categorizedList.Count);
                thisItem = categorizedList[randomIdx];
            }

            else
                thisItem = DataGetter.Instance.ItemList[0];
        }

        return thisItem;
    }
}
