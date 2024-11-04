using System;
using System.Collections.Generic;
using UnityEngine;

public class EvalSubManager
{
    public static event Action<string> OnChatDataUpdated;
    public static event Action<float, ItemInfo> OnItemInit;

    public class NpcEvaluation : InitData
    { //마지막에, 이름, MBTI 타입, 나이, 성별, 키워드, 사고 싶은 물건, 판 물건, 평가
        public string npcName;
        public int npcAge;
        public bool npcSex;
        public string npcKeyword;

        public bool isSuccess;
        public string wantItemName;
        public string boughtItemName;

        public string summary;
    }

    // NpcEvaluation 타입을 저장하는 Dictionary를 정의
    public Dictionary<int, NpcEvaluation> NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();

    public int currentNpcId = 0;
    public void InitNpcDict(InitData initData)
    {
        NpcInfo npc = Managers.Data.npcList[initData.npcID];
        ItemInfo item = Managers.Data.itemList[initData.itemID];

        NpcEvaluation _npcEvaluation = new NpcEvaluation
        {
            npcID = npc.NpcID,
            npcName = npc.NpcName,
            npcAge = npc.NpcAge,
            npcSex = (npc.NpcSex == "female"),
            npcKeyword = npc.KeyWord,

            itemID = initData.itemID,
            boughtItemName = item.ObjName,
            wantItemName = string.Empty,
            
            summary = string.Empty
        };

        if (currentNpcId != 0)
        {
        //    Debug.Log($"+++++++++그 전 애{NpcEvalDict[currentNpcId].npcName} 평가 {NpcEvalDict[currentNpcId].npcEvaluation}");
        }

        currentNpcId = npcId;

        if (NpcEvalDict.ContainsKey(currentNpcId))
        {
            NpcEvalDict[currentNpcId] = _npcEvaluation;
        }
        else
        {
            NpcEvalDict.Add(currentNpcId, _npcEvaluation);
        }

        OnChatDataUpdated?.Invoke(nameof(currentNpcId));
    }

    public void AddEvaluation(string npcEvaluation)
    {
        NpcEvalDict[currentNpcId].npcEvaluation = npcEvaluation;
        Debug.Log($"Eval 2. 평가 업데이트 {npcEvaluation}");
    }

    //--------------------------------------------------
    public ItemInfo itemInfo { get; set; }
    
    public void InitItem(float userSuggest, ItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;
        OnItemInit?.Invoke(userSuggest, itemInfo);
    }

    public void AddItemPriceSold()
    {
        NpcEvalDict[currentNpcId].item = itemInfo.ObjName;
        NpcEvalDict[currentNpcId].itemID = itemInfo.ObjID;
        Managers.Inven.RemoveFromInventory(itemInfo.ObjID);
        Debug.Log($"아이템 팔렸습니다 {NpcEvalDict[currentNpcId].price}");
        OnChatDataUpdated?.Invoke(nameof(itemInfo));
    }
}
