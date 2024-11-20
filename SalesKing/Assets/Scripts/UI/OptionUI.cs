using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle; // 전체 화면 선택 Toggle (Inspector에서 연결)

    private void Start()
    {
        if (fullscreenToggle != null)
        {
            // Toggle의 상태 변경 시 이벤트 연결
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);
        }
        else
        {
            Debug.LogError("Fullscreen Toggle이 연결되지 않았습니다. Inspector에서 연결해주세요.");
        }
    }

    private void OnDestroy()
    {
        if (fullscreenToggle != null)
        {
            // 이벤트 연결 해제
            fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggle);
        }
    }

    // Toggle 상태 변경 시 호출되는 함수
    private void OnFullscreenToggle(bool isOn)
    {
        if (isOn)
        {
            // 전체화면 활성화
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Debug.Log("전체 화면 모드로 변경되었습니다.");
        }
        else
        {
            // 전체화면 비활성화 (창모드)
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log("창모드로 변경되었습니다.");
        }
    }
}
