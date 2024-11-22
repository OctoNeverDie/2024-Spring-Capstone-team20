using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle; // 전체 화면 선택 Toggle (Inspector에서 연결)
    [SerializeField] private Slider musicSlider, sfxSlider;

    private void Start()
    {
        // 전체화면 Toggle 설정
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);
        }
        else
        {
            Debug.LogError("Fullscreen Toggle이 연결되지 않았습니다. Inspector에서 연결해주세요.");
        }

        // 슬라이더 이벤트 동적으로 추가
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }
        else
        {
            Debug.LogError("Music Slider가 연결되지 않았습니다.");
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }
        else
        {
            Debug.LogError("SFX Slider가 연결되지 않았습니다.");
        }
    }

    private void OnDestroy()
    {
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggle);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
        }
    }

    // Toggle 상태 변경 시 호출되는 함수
    private void OnFullscreenToggle(bool isOn)
    {
        if (isOn)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Debug.Log("전체 화면 모드로 변경되었습니다.");
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log("창모드로 변경되었습니다.");
        }
    }

    // 음악 볼륨 슬라이더 변경 시 호출
    private void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.MusicVolume(value);
    }

    // 효과음 볼륨 슬라이더 변경 시 호출
    private void OnSFXSliderChanged(float value)
    {
        AudioManager.Instance.SFXVolume(value);
    }
}
