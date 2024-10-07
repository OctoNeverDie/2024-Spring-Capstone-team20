using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VariableList
{
    public static event Action<string> OnVariableUserUpdated;
    public static event Action<string> OnVariableGptUpdated;
    public static event Action<string> OnVariableChanged;

    private static string _s_UserAnswer;
    private static string _s_GptAnswer;
    private static string _s_GptReaction;
    public static string S_GptReaction
    {
        get => _s_GptReaction;
        set
        {
            _s_GptReaction = value;
            OnVariableChanged?.Invoke(nameof(S_GptReaction));
        }
    }
    public static string S_UserAnswer
    {
        get => _s_UserAnswer;
        set
        {
            _s_UserAnswer = value;
            OnVariableUserUpdated?.Invoke(_s_UserAnswer);
        }
    }
    public static string S_GptAnswer
    {
        get => _s_GptAnswer;
        set
        {
            _s_GptAnswer = value;
            OnVariableGptUpdated?.Invoke(_s_GptAnswer);
        }
    }

    public static void ClearStaticData()
    {
        Debug.Log($"지워 : 2 S_currentNpcId {S_currentNpcId} ");
        _s_UserAnswer = "";
        _s_GptAnswer = "";
        S_currentNpcId = 0;

        S_itemInfo = new ItemInfo();
        S_ThingToBuy = "";
        Debug.Log($"지워 : 3 S_currentNpcId {S_currentNpcId} ");
    }
    //--------------------------------------------------
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
    public static int S_currentNpcId { get; private set; }
    // NpcEvaluation 타입을 저장하는 Dictionary를 정의
    public static Dictionary<int, NpcEvaluation> S_NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();
    
    public static void InitNpcDict(int npcId, string npcName, int npcAge, bool npcSex)
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

        Debug.Log($"지워 : S_currentNpcId {S_currentNpcId} ");
        OnVariableChanged?.Invoke(nameof(S_currentNpcId));
    }

    public static void AddEvaluation(string npcEvaluation) 
    {
        S_NpcEvalDict[S_currentNpcId].npcEvaluation = npcEvaluation;

        OnVariableChanged?.Invoke(nameof(S_NpcEvalDict));
    }

    //--------------------------------------------------
    public static event Action<float, ItemInfo> OnItemInit;
    public static ItemInfo S_itemInfo { get; set; }
    public static string S_ThingToBuy { get; set; }
    public static void InitItem(float userSuggest, ItemInfo itemInfo)
    { 
        S_itemInfo = itemInfo;

        OnItemInit?.Invoke(userSuggest, S_itemInfo);
    }

    public static void AddItemPriceSold(float price)
    {
        S_NpcEvalDict[S_currentNpcId].item = S_itemInfo.ObjName;
        S_NpcEvalDict[S_currentNpcId].price = price;

        OnVariableChanged?.Invoke(nameof(S_itemInfo));
    }
}
