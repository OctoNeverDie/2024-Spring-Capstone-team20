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
    public Dictionary<int, NpcEvaluation> S_NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();

    public int S_currentNpcId = 0;
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

        S_currentNpcId = npcId;

        if (S_NpcEvalDict.ContainsKey(S_currentNpcId))
        {
            S_NpcEvalDict[S_currentNpcId] = _npcEvaluation;
        }
        else
        {
            S_NpcEvalDict.Add(S_currentNpcId, _npcEvaluation);
        }
    }

    public void InitializeNpcEvaluations()
    {
        // 임시 데이터 추가
        S_NpcEvalDict.Clear();  // 기존 데이터 초기화
        S_NpcEvalDict.Add(1, new NpcEvaluation { npcID = 1, npcName = "김철수", npcAge = 25, npcSex = false, npcEvaluation = "친절하고 상냥해요." });
        S_NpcEvalDict.Add(2, new NpcEvaluation { npcID = 2, npcName = "이영희", npcAge = 30, npcSex = true, npcEvaluation = "활발하고 재미있어요." });
        S_NpcEvalDict.Add(3, new NpcEvaluation { npcID = 3, npcName = "박민수", npcAge = 22, npcSex = false, npcEvaluation = "조용하고 신중해요." });
    }


    public void AddEvaluation(string npcEvaluation)
    {
        S_NpcEvalDict[S_currentNpcId].npcEvaluation = npcEvaluation;
    }

    //--------------------------------------------------
    public static event Action<float, ItemInfo> OnItemInit;
    public ItemInfo S_itemInfo { get; set; }
    public string S_ThingToBuy { get; set; }
    public void InitItem(float userSuggest, ItemInfo itemInfo)
    {
        S_itemInfo = itemInfo;

        OnItemInit?.Invoke(userSuggest, S_itemInfo);
    }

    public void AddItemPriceSold(float price)
    {
        S_NpcEvalDict[S_currentNpcId].item = S_itemInfo.ObjName;
        S_NpcEvalDict[S_currentNpcId].price = price;
    }

    public void PrintDictionary()
    {
        foreach (var kvp in S_NpcEvalDict)
        {
            int npcId = kvp.Key;
            NpcEvaluation npcEval = kvp.Value;
            Debug.Log($"NPC ID: {npcId}, Details: {npcEval.npcID}+{npcEval.npcEvaluation}");
        }
    }
}
