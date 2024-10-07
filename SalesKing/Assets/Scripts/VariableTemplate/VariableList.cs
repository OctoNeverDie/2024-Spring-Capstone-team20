using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VariableList
{
    public static event Action<string> OnVariableUserUpdated;
    public static event Action<string> OnVariableGptUpdated;
    private static string _s_UserAnswer;
    private static string _s_GptAnswer;
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
        _s_UserAnswer = "";
        _s_GptAnswer = "";
        _s_currentNpcId = 0;

        S_itemInfo = new ItemInfo();
        S_ThingToBuy = "";
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
    private static int _s_currentNpcId;
    // NpcEvaluation 타입을 저장하는 Dictionary를 정의
    public static Dictionary<int, NpcEvaluation> S_NpcEvalDict { get; } = new Dictionary<int, NpcEvaluation>();
    
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

        _s_currentNpcId = npcId;

        if (S_NpcEvalDict.ContainsKey(_s_currentNpcId))
        {
            S_NpcEvalDict[_s_currentNpcId] = _npcEvaluation;
        }
        else
        {
            S_NpcEvalDict.Add(_s_currentNpcId, _npcEvaluation);
        }
    }

    public static void AddEvaluation(string npcEvaluation) 
    {
        S_NpcEvalDict[_s_currentNpcId].npcEvaluation = npcEvaluation;
    }

    public static bool CheckEvaluationIsAlready()
    {
        if (S_NpcEvalDict[_s_currentNpcId].npcEvaluation == null)
            return true;
        return false;
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
        S_NpcEvalDict[_s_currentNpcId].item = S_itemInfo.ObjName;
        S_NpcEvalDict[_s_currentNpcId].price = price;
    }
}
