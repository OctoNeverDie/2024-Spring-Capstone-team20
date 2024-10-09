using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummarySpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;  // 인스펙터에서 NPC 프리팹을 지정
    [SerializeField] private Transform gridParent;

    private void Start()
    {
        SpawnEntities();
    }

    void SpawnEntities()
    {
        // S_NpcEvalDict에서 NPC 정보를 받아옴
        foreach (var npcEval in Managers.Chat.EvalManager.NpcEvalDict.Values)
        {
            GameObject newNpc = Instantiate(npcPrefab, gridParent);

            // NPC 정보 동적으로 설정
            newNpc.transform.Find("NpcName").GetComponent<TextMeshProUGUI>().text = "이름: " + npcEval.npcName;
            newNpc.transform.Find("NpcSex").GetComponent<TextMeshProUGUI>().text = "성별: " + (npcEval.npcSex ? "여성" : "남성"); // npcSex의 boolean 값을 텍스트로 변환
            newNpc.transform.Find("NpcAge").GetComponent<TextMeshProUGUI>().text = "나이: " + npcEval.npcAge.ToString() + "세";

            // 요약 정보 표시
            newNpc.transform.Find("Summary").GetComponent<TextMeshProUGUI>().text = npcEval.npcEvaluation;  // 평가 정보 표시
        }
    }

}
