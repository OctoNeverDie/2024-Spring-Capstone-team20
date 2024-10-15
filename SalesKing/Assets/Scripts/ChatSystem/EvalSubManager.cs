using System;
using System.Collections.Generic;
using UnityEngine;

public class EvalSubManager
{

    public static event Action<string> OnChatDataUpdated;
    public static event Action<float, ItemInfo> OnItemInit;

    public class NpcEvaluation
    {
        public int npcID;
        public string npcName;
        public int npcAge;
        public bool npcSex; //female is true
        public string npcKeyword;
        public string item;
        public int itemID;
        public float price;
        public string npcEvaluation;
    }

    // NpcEvaluation 타입을 저장하는 Dictionary를 정의
    public Dictionary<int, NpcEvaluation> NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();

    public int currentNpcId = 0;
    public void InitNpcDict(int npcId, string npcName, int npcAge, bool npcSex, string keyWord)
    {
        NpcEvaluation _npcEvaluation = new NpcEvaluation
        {
            npcID = npcId,
            npcName = npcName,
            npcAge = npcAge,
            npcSex = npcSex,
            npcKeyword = keyWord,
            item = string.Empty,
            itemID = 0,
            price = 0.0f,
            npcEvaluation = string.Empty
        };

        if (currentNpcId != 0)
        {
            Debug.Log($"+++++++++그 전 애{NpcEvalDict[currentNpcId].npcName} 평가 {NpcEvalDict[currentNpcId].npcEvaluation}");
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
    public string ThingToBuy { get; set; }
    
    //아이템 맨처음 고르고, user의 첫 제시가 나옴
    public void InitItem(float userSuggest, ItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;
        OnItemInit?.Invoke(userSuggest, itemInfo);
    }

    public void AddItemPriceSold(float price= -1f)
    {
        if (price == -1f)
        {
            price = NpcEvalDict[currentNpcId].price;
            Debug.Log($"딜버튼 : 아이템 팔렸습니다 {price}");
        }

        NpcEvalDict[currentNpcId].item = itemInfo.ObjName;
        NpcEvalDict[currentNpcId].itemID = itemInfo.ObjID;
        NpcEvalDict[currentNpcId].price = price;
        Debug.Log($"아이템 팔렸습니다 {price}");
        OnChatDataUpdated?.Invoke(nameof(itemInfo));
    }
    public void UpdateSuggestInEval(float suggest)
    {
        NpcEvalDict[currentNpcId].price = suggest;
    }
    /*
    public void PrintDictionary()
    {
        foreach (var kvp in NpcEvalDict)
        {
            int npcId = kvp.Key;
            NpcEvaluation npcEval = kvp.Value;
            Debug.Log($"NPC ID: {npcId}, Details: {npcEval.npcID}+{npcEval.npcEvaluation}");
        }
    }
    */
}
