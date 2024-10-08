using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariableCheck : MonoBehaviour
{
    TextMeshProUGUI[] variables;
    bool isNew = false;

    // Start is called before the first frame update
    void Awake()
    {
        variables = this.GetComponentsInChildren<TextMeshProUGUI>();
        InitVariables();

        ChatManager.OnNumberUpdated += UpdateTurnSuggest;

        VariableList.OnVariableUserUpdated += UserLog;
        VariableList.OnVariableChanged += NpcProfileEvalLog;
    }


    private void UpdateTurnSuggest(int turn, float userSuggest, float npcSuggest)
    {
        Debug.Log($"1 Turn 업데이트 {turn} {userSuggest}  {npcSuggest}");
        variables[10].text = $"턴 : {turn}";
        variables[1].text = $"userSuggest : {userSuggest}";
        variables[2].text = $"npcSuggest: {npcSuggest}";
    }

    private void UserLog(string user_input)
    {
        Debug.Log($"2 유저인풋 업데이트{user_input}");
        variables[3].text = $"userReact : {user_input}";
    }

    private void NpcProfileEvalLog(string variableName)
    {
        switch (variableName)
        {
            case nameof(VariableList.S_currentNpcId):
                Debug.Log("3 npc 프로필 업데이트");
                variables[5].text = $"npc: {VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].npcID}+" +
                    $"{VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].npcName}+" +
                    $"{ VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].npcAge}+" +
                    $"{ VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].npcSex}";
                break;

            case nameof(VariableList.S_NpcEvalDict):
                Debug.Log($"4 평가 업데이트 {VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].npcEvaluation}");
                variables[6].text = $"npcEval:{VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].npcEvaluation}";
                break;

            case nameof(VariableList.S_itemInfo):
                Debug.Log($"5 판가격,아이템 업데이트{VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].item}, {VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].price}");
                variables[7].text = $"산물건: {VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].item}";
                variables[8].text = $"판가격: {VariableList.S_NpcEvalDict[VariableList.S_currentNpcId].price}";
                break;
            case nameof(VariableList.S_GptReaction):
                Debug.Log($"6 npc 리액션 업데이트{VariableList.S_GptReaction}");
                variables[4].text = $"npcReact: {VariableList.S_GptReaction}";
                break;
            case nameof(VariableList.S_ThingToBuy):
                Debug.Log($"7 사고픈물건 업데이트{VariableList.S_ThingToBuy}");
                variables[9].text = $"사고픈물건: {VariableList.S_ThingToBuy}";
                break;
        }
    }

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
}
