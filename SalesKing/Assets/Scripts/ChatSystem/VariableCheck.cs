using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariableCheck : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI userSuggestText;
    [SerializeField]
    TextMeshProUGUI npcSuggestText;
    [SerializeField]
    TextMeshProUGUI sellingItemText;
    [SerializeField]
    TextMeshProUGUI sellingItemFirstCostText;

    void Awake()
    {
        //variables = this.GetComponentsInChildren<TextMeshProUGUI>();
        //InitVariables();
        ChatManager.OnNumberUpdated -= UpdateTurnSuggest;
        ChatManager.OnNumberUpdated += UpdateTurnSuggest;

        ReplySubManager.OnReplyUpdated -= ReplyLog;
        ReplySubManager.OnReplyUpdated += ReplyLog;

        EvalSubManager.OnChatDataUpdated -= NpcEval;
        EvalSubManager.OnChatDataUpdated += NpcEval;
    }

    private void OnEnable()
    {
        Clear();
    }
    //Action : ChatManager.OnNumberUpdated
    private void UpdateTurnSuggest(int _, float npcSuggest, float userSuggest)
    {
        userSuggestText.text = userSuggest.ToString();
        npcSuggestText.text = npcSuggest.ToString();
    }

    private void Clear()
    {
        sellingItemText.text = "-";
        sellingItemFirstCostText.text = "-";
        userSuggestText.text = "-";
        npcSuggestText.text = "-";
    }

    //Action : ReplySubManager.OnReplyUpdated
    private void ReplyLog(string type, string input)
    {
        var ReplyManager = Managers.Chat.ReplyManager;

        switch (type)
        {
            case nameof(ReplyManager.UserAnswer) :
                //UIManager.ui_chatting.SetUserAnswerText(input);
                Debug.Log($"네 대답 잘 감{input}");
                break;

            case nameof(ReplyManager.GptReaction):
                Debug.Log($"걔 대답 잘 옴{input}");
                //UIManager.ui_chatting.SetNPCAnswerText(ReplyManager.GptReaction);
                break;
        }
    }

    //Action : EvalSubManager.OnItemInit
    private void NpcInititem(float userSuggest, ItemInfo itemInfo)
    {
        //item init -> chatbargain : 패널에 보여줄 때
        //판매할 물건, 판매하는 물건, 원가, 유저 첫 제시가
        sellingItemText.text = itemInfo.ObjName;
        //sellingItemFirstCostText.text = itemInfo.defaultPrice.ToString();
        userSuggestText.text = userSuggest.ToString();
        npcSuggestText.text = "?";
    }

    //Action : EvalSubManager.OnChatDataUpdated, (npcinit)currentNpcId/(evaluation)NpcEvalDict/(bought item)itemInfo
    private void NpcEval(string type)
    {
        var EvalManager = Managers.Chat.EvalManager;
        
        int DictId = EvalManager.currentNpcId;

        if (DictId == 0)
            return;

        var npcEvalDict = EvalManager.NpcEvalDict[DictId];

        switch (type)
        {
            case nameof(EvalManager.currentNpcId):
                Debug.Log("Eval 1. npc 프로필 업데이트"+$"npc: {npcEvalDict.npcID}+" +
                    $"{npcEvalDict.npcName}+" +
                    $"{npcEvalDict.npcAge}+" +
                    $"{npcEvalDict.npcSex}");
                break;
        }
    }
}
