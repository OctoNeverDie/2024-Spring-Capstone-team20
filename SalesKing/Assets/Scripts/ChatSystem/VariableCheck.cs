using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariableCheck : MonoBehaviour
{
    //TextMeshProUGUI[] variables;

    [SerializeField]
    TextMeshProUGUI userSuggestText;
    [SerializeField]
    TextMeshProUGUI npcSuggestText;


    void Awake()
    {
        //variables = this.GetComponentsInChildren<TextMeshProUGUI>();
        //InitVariables();

        ChatManager.OnNumberUpdated += UpdateTurnSuggest;

        ReplySubManager.OnUserReplyUpdated += UserLog;
        ReplySubManager.OnChatDataUpdated += NpcProfileEvalLog;
    }


    private void UpdateTurnSuggest(int turn, float userSuggest, float npcSuggest)
    {
        Debug.Log($"1 Turn 업데이트 {turn} {userSuggest}  {npcSuggest}");
        //variables[10].text = $"턴 : {turn}";
        //variables[1].text = $"userSuggest : {userSuggest}";
        //variables[2].text = $"npcSuggest: {npcSuggest}";
    }

    private void UserLog(string user_input)
    {
        //variables[3].text = $"userReact : {user_input}";
        Managers.UI.SetUserAnswerText(user_input);
    }
    
    private void NpcProfileEvalLog(string variableName)
    {
        var EvalManager = Managers.Chat.EvalManager;
        var ReplyManager = Managers.Chat.ReplyManager;

        int DictId = EvalManager.currentNpcId;

        if (DictId == 0)
            return;

        var variableList = EvalManager.NpcEvalDict[DictId];

        switch (variableName)
        {
            case nameof(EvalManager.currentNpcId):
                Debug.Log("3 npc 프로필 업데이트");
                /*
                variables[5].text = $"npc: {variableList.npcID}+" +
                    $"{variableList.npcName}+" +
                    $"{variableList.npcAge}+" +
                    $"{variableList.npcSex}";
                */
                break;

            case nameof(EvalManager.NpcEvalDict):
                Debug.Log($"4 평가 업데이트 {variableList.npcEvaluation}");
                //variables[6].text = $"npcEval:{variableList.npcEvaluation}";
                break;

            case nameof(EvalManager.itemInfo):
                Debug.Log($"5 판가격,아이템 업데이트{variableList.item}, {variableList.price}");
                //variables[7].text = $"산물건: {variableList.item}";
                //variables[8].text = $"판가격: {variableList.price}";
                break;
            case nameof(ReplyManager.GptReaction):
                Debug.Log($"6 npc 리액션 업데이트{ReplyManager.GptReaction}");
                //variables[4].text = $"npcReact: {VariableList.GptReaction}";
                Managers.UI.SetNPCAnswerText(ReplyManager.GptReaction);
                break;
            case nameof(EvalManager.ThingToBuy):
                Debug.Log($"7 사고픈물건 업데이트{EvalManager.ThingToBuy}");
                //variables[9].text = $"사고픈물건: {VariableList.ThingToBuy}";
                break;
        }
    }

    /*
    private void InitVariables()
    {
        variables[0].text = "턴";
        variables[1].text = "userSuggest";
        variables[2].text = "npcSuggest";
        variables[3].text = "userReact";
        variables[4].text = "npcReact";
        variables[5].text = "npc";
        variables[6].text = "npcEval";
        variables[7].text = "산물건";
        variables[8].text = "판가격";
        variables[9].text = "사고픈물건";
        variables[10].text = "턴";
    }
    */
}
