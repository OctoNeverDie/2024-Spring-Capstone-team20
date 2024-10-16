using UnityEngine;
using UnityEngine.UI;

public class TestGage : MonoBehaviour
{
    [SerializeField] private Slider nowSlider; // 인스펙터에서 슬라이더 할당

    private const int initTurn = 7;
    public int _turn = initTurn;

    void Start()
    {

        ChatManager.OnNumberUpdated -= TurnUpdated;
        ChatManager.OnNumberUpdated += TurnUpdated;

        UpdateSlider(initTurn, nowSlider);
    }

    private void TurnUpdated(int turn, float _, float __)
    {
        if (turn <= -20)
            turn = 0;
        else if (turn <= 0)
            turn = 1;
        UpdateSlider(turn, nowSlider);
    }

    private void UpdateSlider(int turn, Slider slider)
    {
        // 턴 수를 0에서 initTurn 사이로 클램프
        int clampedTurn = Mathf.Clamp(turn, 0, initTurn);
        // 필량을 0에서 1 사이로 계산
        float fillAmount = (float)clampedTurn / initTurn;

        if (slider != null)
        {
            slider.value = fillAmount;
        }
        else
        {
            Debug.LogWarning("슬라이더가 할당되지 않았습니다.");
        }
    }
    void OnDestroy()
    {
        // 메모리 누수를 방지하기 위해 이벤트에서 리스너 제거
        ChatManager.OnNumberUpdated -= TurnUpdated;
    }
}
