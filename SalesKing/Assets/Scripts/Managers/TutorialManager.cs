using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject guidePanel;           // 첫 안내 패널
    public Text MissionText;                // 미션 텍스트 (Mission Panel 안의 Text)

    private bool hasTalkedToCustomer = false;

    void Start()
    {
        ShowInitialMission();
    }

    // 첫 미션 보여주기
    private void ShowInitialMission()
    {
        MissionText.text = "물건을 살 고객에게 말을 거세요.";  // 첫 번째 미션 텍스트 설정
        guidePanel.SetActive(true);  // 첫 안내 패널 표시
    }

    // 고객과 대화를 시작했을 때 호출될 함수
    public void OnTalkToCustomer()
    {
        MissionText.text = "";  // 미션 텍스트 업데이트
    }
}
