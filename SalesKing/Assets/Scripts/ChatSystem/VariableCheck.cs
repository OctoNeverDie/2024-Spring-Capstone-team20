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
    TextMeshProUGUI itemYouSaid;
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

        EvalSubManager.OnItemInit -= NpcInititem;
        EvalSubManager.OnItemInit += NpcInititem;
    }

    //Action : ChatManager.OnNumberUpdated
    private void UpdateTurnSuggest(int turn, float userSuggest, float npcSuggest)
    {
        if(turn <= 0)
        {
            Clear();
            return;
        }
        userSuggestText.text = userSuggest.ToString();
        npcSuggestText.text = npcSuggest.ToString();
    }

    private void Clear()
    {
        itemYouSaid.text = "";
        sellingItemText.text = "";
        sellingItemFirstCostText.text = "";
        userSuggestText.text = "";
        npcSuggestText.text = "";
    }

    //Action : ReplySubManager.OnReplyUpdated
    private void ReplyLog(string type, string input)
    {
        var ReplyManager = Managers.Chat.ReplyManager;

        switch (type)
        {
            case nameof(ReplyManager.UserAnswer) :
                Managers.UI.SetUserAnswerText(input);
                break;

            case nameof(ReplyManager.GptAnswer):
                Managers.UI.SetNPCAnswerText(input);
                break;

            case nameof(ReplyManager.GptReaction):
                Managers.UI.SetNPCAnswerText(ReplyManager.GptReaction);
                break;
        }
    }

    //Action : EvalSubManager.OnItemInit
    private void NpcInititem(float userSuggest, ItemInfo itemInfo)
    {
        //item init -> chatbargain : 패널에 보여줄 때
        Debug.Log($"7 사고픈물건 업데이트{Managers.Chat.EvalManager.ThingToBuy}");
        Debug.Log($"5 팔가격,아이템 업데이트{userSuggest}, {itemInfo.ObjName}");
        //판매할 물건이랑, 판매하는 물건 두개 알려줘야혀................
        //1차 프롬프트에서 사탕 팔게요~ 하는데 item init에서 캣타워. 선택하면 애매해잖아
        //그래서 사고픈물건은 사탕, 내가 실제로 인벤에서 선택한 물건은 캣타워.라고 출려갷야해
        sellingItemText.text = itemInfo.ObjName;
        sellingItemFirstCostText.text = itemInfo.defaultPrice.ToString();
        if(Managers.Chat.EvalManager.ThingToBuy != null)
            itemYouSaid.text = Managers.Chat.EvalManager.ThingToBuy.ToString();
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

            case nameof(EvalManager.NpcEvalDict):
                Debug.Log($"Eval 2. 평가 업데이트 {npcEvalDict.npcEvaluation}");
                break;

            case nameof(EvalManager.itemInfo):
                //chatbargain -> success : item이 user 지갑 주머니에 들어와있을 때
                Debug.Log($"5 판가격,아이템 업데이트{npcEvalDict.item}, {npcEvalDict.price}");
                //Managers.Inven.RemoveFromInventory(npcEvalDict.itemID);
                Managers.Cash.AddCash(npcEvalDict.price);
                break;            
        }
    }
}
