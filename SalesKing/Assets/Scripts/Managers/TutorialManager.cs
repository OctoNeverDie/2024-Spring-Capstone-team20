using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private GameObject missionPanel;         // 안내 패널 (동적 할당)
    private TextMeshProUGUI missionText;   // 미션 텍스트 (동적 할당)


    private void Start()
    {
        // 씬 로드 시 호출: MissionPanel과 missionText 찾기
        FindGuidePanel();
        SetInitialMissionText();  // 초기 미션 텍스트 설정
    }

    // Canvas 하위에서 guidePanel과 missionText 찾기
    private void FindGuidePanel()
    {
        GameObject canvas = GameObject.Find("Canvas");  // Canvas 찾기
        if (canvas != null)
        {
            missionPanel = canvas.transform.Find("MissionPanel").gameObject;  // MissionPanel 찾기
            if (missionPanel != null)
            {
                missionText = missionPanel.transform.Find("MissionText").GetComponent<TextMeshProUGUI>();  // MissionText 찾기
            }
        }
    }

    // 씬 이름에 따라 초기 미션 텍스트 설정
    private void SetInitialMissionText()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;  // 현재 씬 이름 가져오기

        if (currentSceneName == "OfficeMap")
        {
            ShowGuide("컴퓨터를 눌러 물건을 사세요.");  // OfficeMap에서 시작할 때
        }
        else if (currentSceneName == "CityMap")
        {
            ShowGuide("물건을 살 고객에게 말을 거세요.");  // CityMap에서 시작할 때
        }
    }

    // 텍스트 업데이트 함수
    public void ShowGuide(string text)
    {
        if (missionText != null)
        {
            missionText.text = text;  // 미션 텍스트 업데이트
            missionPanel.SetActive(true);  // 안내 패널 활성화
        }
    }

    // 특정 이벤트 시 텍스트 변경
    public void OnRecord()
    {
        ShowGuide("스페이스바를 눌러 대화를 녹음하세요.");
    }

    public void OnPersuadeToCustomer()
    {
        ShowGuide("NPC와 계속 대화를 나눠보세요.");
    }

    public void OnClickComputer()
    {
        ShowGuide("컴퓨터를 클릭하여 작업을 시작하세요.");
    }

    public void OnTalkToSecretary()
    {
        ShowGuide("비사에게 말을 거세요.");
    }

    public void OnGotoCityMap()
    {
        ShowGuide("명심하세요. 당신은 오늘 200달러를 벌어야합니다.");
    }


    // 패널 숨기기
    public void HideGuide()
    {
        missionPanel.SetActive(false);  // 안내 패널 비활성화
    }
}
