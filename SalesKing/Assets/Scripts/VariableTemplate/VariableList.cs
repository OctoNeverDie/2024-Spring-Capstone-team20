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
    private static int S_currentNpcId;
    public static Dictionary<int, NpcEvaluation> S_NpcEvalDict { get; }
    public static void InitNpcDict(int npcId, string npcName, int npcAge, bool npcSex) 
    {
        NpcEvaluation _npcEvaluation = new NpcEvaluation();

        S_currentNpcId = npcId;
        _npcEvaluation.npcID = S_currentNpcId;
;
        _npcEvaluation.npcName = npcName;
        _npcEvaluation.npcAge = npcAge;
        _npcEvaluation.npcSex = npcSex;

        S_NpcEvalDict.Add(S_currentNpcId, _npcEvaluation);
    }

    public static void AddEvaluation(string npcEvaluation) 
    {
        S_NpcEvalDict[S_currentNpcId].npcEvaluation = npcEvaluation;
    }
    public static void AddItemPriceSold(string item, int price) 
    {
        S_NpcEvalDict[S_currentNpcId].item = item;
        S_NpcEvalDict[S_currentNpcId].price = price;
    }
    //--------------------------------------------------
    public static event Action<float, ItemInfo> OnItemInit;

    private static float _s_userSuggest;
    private static ItemInfo _s_itemInfo;
    public static string S_ThingToBuy { get; set; }
    public static void InitItem(float userSuggest, ItemInfo itemInfo)
    { 
        _s_userSuggest = userSuggest;
        _s_itemInfo = itemInfo;

        OnItemInit?.Invoke(_s_userSuggest, _s_itemInfo);
    }


    #region Legacy

    public static int S_Affinity { get; set; }
    public static int S_Usefulness { get; set; }
    public static int S_AlphaPrice { get; set; }
    public static int S_DefaultPrice { get; set; }


    //4 Things to send-------------------------------
    public static int S_ExpectedPrice { get; set; }
    public static int S_AffordablePrice { get; set; }
    public static string S_Relationship { get; set; }

    static VariableList()
    {
        S_Affinity = 0;
        S_Usefulness = 0;

        S_DefaultPrice = 10;
        //S_DefaultPrice = (int)Math.Ceiling(S_DefaultPrice * 1.1); //이거는 나중에 
        S_ExpectedPrice = S_DefaultPrice; 
        S_AlphaPrice = 0;
        S_AffordablePrice = S_ExpectedPrice+ S_AlphaPrice;

        S_UserAnswer = "";
        S_Relationship = "neutral";
    }
    #endregion
}
