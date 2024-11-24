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
        _sendChatType = SendChatType.ChatInit;
        npc = Chat.ThisNpc;
        playerItem = GetRandItem();

        ShowFront();
        SendBack();
    }
    public override void Exit()
    {
        UpdateEvaluation();
    }
    private void UpdateEvaluation()
    {
        Chat.Eval.InitEvalDictNpc(npc.NpcID, playerItem.ObjID);
    }

    private void ShowFront()
    {
        Chat.ActivatePanel(_sendChatType, playerItem, npc.NpcName);
    }

    private void SendBack()
    {
        if (npc.NpcID == 0)
        {
            Chat.FirstNpc();
            return;
        }
        
        string _userSend = MakeUserSend(npc) + "\n" + MakeMbtiSend(npc.Mbtis);

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}. {_userSend}");
        ServerManager.Instance.GetGPTReply("$start", _sendChatType, _userSend);
    }

    private ItemInfo GetRandItem()
    {
        List<ItemInfo> categorizedList= DataGetter.Instance.CategorizedItems[npc.ItemCategory];
        foreach (var cate in categorizedList)
        {
            Debug.Log($"items : {npc.ItemCategory}, {cate.Category}, {cate.ObjName}");
        }
        int randomIdx;
        if (categorizedList.Count > 0)
            randomIdx = Random.Range(0, categorizedList.Count);
        else
            return DataGetter.Instance.ItemList[0];

        return categorizedList[randomIdx];
    }

    //감정형 호소, 매력형 어필, 논리형 설득, 아부형 칭찬
    private string MakeMbtiSend(int[] mbtiPrefers)
    {
        string likeType = "";
        string likeType2 = "";
        string disLikeType = "";
        string disLikeType2 = "";

        for (int i = 0; i < mbtiPrefers.Length; i++)
        {
            if (mbtiPrefers[i] == 1)
            {
                switch (i)
                {
                    case 0:
                        likeType += " \"감성형\",";
                        likeType2 += " 감성적 호소,";
                        break;
                    case 1:
                        likeType += " \"논리형\",";
                        likeType2 += " 논리적 설득,";
                        break;
                    case 2:
                        likeType += " \"아부형\",";
                        likeType2 += " 아부섞인 칭찬,";
                        break;
                    case 3:
                        likeType += " \"유혹형\",";
                        likeType2 += " 매력적인 어필,";
                        break;
                }
            }

            else if (mbtiPrefers[i] == -1)
            {
                switch (i)
                {
                    case 0:
                        disLikeType += "\"감성형\",";
                        disLikeType2 += "감성적 호소,";
                        break;
                    case 1:
                        disLikeType += "\"논리형\",";
                        disLikeType2 += "논리적 설득,";
                        break;
                    case 2:
                        disLikeType += "\"아부형\",";
                        disLikeType2 += "아부섞인 칭찬,";
                        break;
                    case 3:
                        disLikeType += "\"유혹형\",";
                        disLikeType2 += "매력적인 어필,";
                        break;
                }
            }
        }
        string introduceGood = "";
        string GoodpointGood = "";
        string GoodpointBad = "";

        string introduceBad = "";
        string BadpointBad = "";

        if (likeType != "")
        {
            introduceGood = $" 너는 {likeType} 설득 유형을 좋아해. 넌 판매자의 {likeType2}에 특히 잘 설득 돼(=설득 수치가 높게 나온다). 이 경우 너한테 필요 없어도 물건을 사고 싶은 마음이 생길 수 있어.";
            GoodpointGood = $" \n판매자의 {likeType2}에 대해 네가 호감을 느꼈다면, 그 정도에 따라 총합 +2에서 +4까지 Persuasion을 출력해줘.";
            GoodpointBad = $" \n판매자의 {likeType2}에 대해 네가 불쾌감을 느꼈다면, 그 정도에 따라 -1에서 -2까지 persuasion을 출력해줘.";
        }
            
        if (disLikeType != "")
        {
            introduceBad = $" 너는 {disLikeType} 설득 유형을 싫어해. 넌 판매자의 {disLikeType2}에 특히 잘 설득 안 돼(=설득 수치가 낮게 나온다). 이 경우 너한테 필요 없으면 물건을 딱히 사고 싶지 않아.";
            BadpointBad = $" \n판매자의 {disLikeType2}에 대해 네가 불쾌감을 느껴 반응하거나 아무 반응 안 해. 정도에 따라 0에서 -3까지 persuasion을 출력해줘.";
        }

        string fixedSentence = "판매자가 다른 설득 유형에 해당하는 답을 한다면, -1 ~ +1 정도의 시큰둥한 반응을 해줘.\r\n- 물론 네 성격과 키워드에 따라서, 호감이나 비호감을 유발하는 말이라면 유형을 무시하고 점수와 반응을 유동적으로 해도 좋아.";

        string result = introduceGood + introduceBad + GoodpointGood + GoodpointBad + BadpointBad + fixedSentence;

        return result;
    }

    private string MakeUserSend(NpcInfo npc)
    {
        string user_send = $"\n NpcName : {npc.NpcName}, NpcSex : {npc.NpcSex}, NpcAge : {npc.NpcAge} "
            + $" KeyWord : {npc.KeyWord}, \nPersonailty : {npc.Personality}\nDialogue Style: {npc.DialogueStyle}\nExample: {npc.Example}"
            +$"\nSituation : {npc.SituationDescription}"
            + $"\n당근에 올린 글: {npc.Concern}\n네가 사려고 한 물건: {npc.WantItem}, 판매자가 가져온 물건: {playerItem.ObjName}\n";

        return user_send;
    }
}
