using UnityEngine;
using TMPro;
using DG.Tweening;


// 상태 인터페이스
public interface ITutorialState
{
    void Enter();  // 상태 진입 시 호출
    void Exit();   // 상태 종료 시 호출
    void Update(); // 매 프레임마다 호출
}

// 고객에게 말을 거는 상태
public class TalkToCustomerState : ITutorialState
{
    private TextMeshProUGUI missionText;
    private GameObject guidePanel;

    public TalkToCustomerState(TextMeshProUGUI missionText, GameObject guidePanel)
    {
        this.missionText = missionText;
        this.guidePanel = guidePanel;
    }

    public void Enter()
    {
        missionText.text = "물건을 살 고객에게 말을 거세요.";  // 첫 번째 미션 텍스트 설정
        guidePanel.SetActive(true);  // 첫 안내 패널 표시
    }

    public void Exit() { }

    public void Update() { }
}

// 대화 녹음 상태
public class RecordingState : ITutorialState
{
    private TextMeshProUGUI missionText;

    public RecordingState(TextMeshProUGUI missionText)
    {
        this.missionText = missionText;
    }

    public void Enter()
    {
        missionText.text = "스페이스바를 눌러 대화를 녹음하세요.";  // 미션 텍스트 업데이트
    }

    public void Exit() { }

    public void Update()
    {
        if (Input.GetButtonDown("STT"))
        {
            missionText.text = "녹음 중...";  // 녹음 중 텍스트 업데이트
        }
        else if (Input.GetButtonUp("STT"))
        {
            missionText.text = "대화 전송 버튼을 눌러 전송하세요.";  // 대화 전송 안내 텍스트 업데이트
        }
    }
}

public class EndState : ITutorialState
{
    private TextMeshProUGUI missionText;
    private GameObject endPanel;  // 하루가 끝났음을 표시하는 패널

    public EndState(TextMeshProUGUI missionText, GameObject endPanel)
    {
        this.missionText = missionText;
        this.endPanel = endPanel;
    }

    public void Enter()
    {
        // 패널을 비활성화 상태로 시작합니다.
        endPanel.SetActive(true);
        endPanel.transform.localScale = Vector3.zero; // 초기 스케일 설정 (0으로)

        // DOTween을 사용하여 패널을 부드럽게 나타나게 합니다.
        endPanel.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            // 2초 후에 내용을 업데이트하고 다시 비활성화
            // 성과 요약 표시 로직 추가
            UpdateSummary();

            // 2초 대기 후 패널을 비활성화합니다.
            DOVirtual.DelayedCall(2f, () =>
            {
                endPanel.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                {
                    endPanel.SetActive(false);  // 패널 비활성화
                });
            });
        });
    }

    private void UpdateSummary()
    {
        // 성과 요약 계산 및 표시 로직을 추가합니다.
        // 예: missionText.text = "오늘 하루 성과: 1000원, 목표 금액: 1500원, 평가: 좋음";
        missionText.text = "오늘 하루 성과: 1000원, 목표 금액: 1500원, 평가: 좋음"; // 예시
    }

    public void Exit()
    {
        endPanel.SetActive(false);  // 종료 시 패널 비활성화
    }

    public void Update()
    {
        // 여기서 필요한 경우 상태 업데이트 로직을 추가할 수 있습니다.
    }
}
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; } // 싱글턴 인스턴스

    public GameObject guidePanel;           // 첫 안내 패널
    public GameObject endPanel;              // 하루 끝 패널
    public TextMeshProUGUI missionText;      // 미션 텍스트 (Mission Panel 안의 Text)

    private ITutorialState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 인스턴스 설정
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있을 경우 삭제
        }
    }
    void Start()
    {
        ChangeState(new TalkToCustomerState(missionText, guidePanel));  // 시작 상태 설정
    }

    void Update()
    {
        currentState?.Update();  // 현재 상태의 업데이트 호출
    }

    public void ChangeState(ITutorialState newState)
    {
        currentState?.Exit();  // 현재 상태 종료
        currentState = newState;  // 새 상태로 변경
        currentState.Enter();  // 새 상태 초기화
    }

    public void OnTalkToCustomer()
    {
        if (currentState == null)
        {
            Debug.LogError("currentState가 null입니다. TutorialManager가 초기화되지 않았습니다.");
            return; // null일 경우 작업을 중지
        }

        if (currentState is RecordingState)
        {
            Debug.LogWarning("이미 대화 녹음 중입니다.");
            return; // 이미 녹음 상태일 때는 아무 작업도 하지 않음
        }

        ChangeState(new RecordingState(missionText)); // 대화 녹음 상태로 변경
    }

    // 대화가 끝났을 때 호출되는 메서드
    public void OnConversationEnd()
    {
        ChangeState(new EndState(missionText, endPanel)); // End 상태로 변경
    }
}
