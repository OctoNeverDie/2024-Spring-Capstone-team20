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
        public string concern;

        public string summary;
    }

    // NpcEvaluation 타입을 저장하는 Dictionary를 정의
    public Dictionary<int, NpcEvaluation> NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();

    public int currentNpcId = 0;
    public NpcEvaluation _npcEvaluation;
    public void InitNpcDict(InitData initData, NpcInfo npc)
    {
        string boughtItemName = Managers.Data.itemList[initData.boughtItemID].ObjName;
        string wantItemName = Managers.Data.itemList[initData.wantItemID].ObjName;
        string concern = Managers.Data.concernList[initData.concernID].Concern;

        _npcEvaluation = new NpcEvaluation
        {
            npcID = npc.NpcID,
            npcName = npc.NpcName,
            npcAge = npc.NpcAge,
            npcSex = (npc.NpcSex == "female"),
            npcKeyword = npc.KeyWord,

            boughtItemName = boughtItemName,
            wantItemName = wantItemName,
            concern = concern,
            
            summary = string.Empty
        };

        currentNpcId = npc.NpcID;

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
        NpcEvalDict[currentNpcId].summary = npcEvaluation;
        Debug.Log($"Eval 평가 업데이트 {npcEvaluation}");
    }
}
