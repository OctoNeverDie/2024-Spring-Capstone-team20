using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalSubManager
{
    public class NpcEvaluation
    {
        public int npcID;
        public string npcName;
        public int npcAge;
        public bool npcSex; //female is true
        public string item;
        public float price;
        public string npcEvaluation;
    }

    // NpcEvaluation 타입을 저장하는 Dictionary를 정의
    public Dictionary<int, NpcEvaluation> NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();

    public int currentNpcId = 0;
    public void InitNpcDict(int npcId, string npcName, int npcAge, bool npcSex)
    {
        NpcEvaluation _npcEvaluation = new NpcEvaluation
        {
            npcID = npcId,
            npcName = npcName,
            npcAge = npcAge,
            npcSex = npcSex,
            item = string.Empty,
            price = 0.0f,
            npcEvaluation = string.Empty
        };

        currentNpcId = npcId;

        if (NpcEvalDict.ContainsKey(currentNpcId))
        {
            NpcEvalDict[currentNpcId] = _npcEvaluation;
        }
        else
        {
            NpcEvalDict.Add(currentNpcId, _npcEvaluation);
        }
    }

    public void InitializeNpcEvaluations()
    {
        // 임시 데이터 추가
        NpcEvalDict.Clear();  // 기존 데이터 초기화
        NpcEvalDict.Add(1, new NpcEvaluation { npcID = 1, npcName = "김철수", npcAge = 25, npcSex = false, npcEvaluation = "친절하고 상냥해요." });
        NpcEvalDict.Add(2, new NpcEvaluation { npcID = 2, npcName = "이영희", npcAge = 30, npcSex = true, npcEvaluation = "활발하고 재미있어요." });
        NpcEvalDict.Add(3, new NpcEvaluation { npcID = 3, npcName = "박민수", npcAge = 22, npcSex = false, npcEvaluation = "조용하고 신중해요." });
    }

    public void AddEvaluation(string npcEvaluation)
    {
        NpcEvalDict[currentNpcId].npcEvaluation = npcEvaluation;
    }

    //--------------------------------------------------
    public static event Action<float, ItemInfo> OnItemInit;
    public ItemInfo itemInfo { get; set; }
    public string ThingToBuy { get; set; }
    public void InitItem(float userSuggest, ItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;
        OnItemInit?.Invoke(userSuggest, itemInfo);
    }

    public void AddItemPriceSold(float price)
    {
        NpcEvalDict[currentNpcId].item = itemInfo.ObjName;
        NpcEvalDict[currentNpcId].price = price;
    }

    public void PrintDictionary()
    {
        foreach (var kvp in NpcEvalDict)
        {
            int npcId = kvp.Key;
            NpcEvaluation npcEval = kvp.Value;
            Debug.Log($"NPC ID: {npcId}, Details: {npcEval.npcID}+{npcEval.npcEvaluation}");
        }
    }
}
