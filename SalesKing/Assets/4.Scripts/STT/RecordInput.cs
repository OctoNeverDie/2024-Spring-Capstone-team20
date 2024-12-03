using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manage
/// 1. STT Slider
/// 2. STT RecordButton
/// 3. STT resultText
/// </summary>
public class RecordInput : MonoBehaviour
{
    [SerializeField] GameObject ResultInputfield;
    [SerializeField] GameObject RecordInputRect;//contains button, button image, slider
    [SerializeField] Sprite recordOn;
    [SerializeField] Sprite recordOff;
    [SerializeField] Sprite muhanRecordOn;
    [SerializeField] GameObject noMicDetected;

    Image recordImg;
    Button recordBtn;
    Slider recordSlider;
    Define.GameMode gameMode;
    STTConnect STTconnect;

    [HideInInspector]
    public bool _isRecording = false;
    private float _currentRecordingTime;
    private Coroutine _recordingCoroutine;

    private void Awake()
    {
        STTconnect = this.GetComponent<STTConnect>();

        recordImg = RecordInputRect.GetComponentInChildren<Image>();
        recordBtn = RecordInputRect.GetComponentInChildren<Button>();
        recordSlider = RecordInputRect.GetComponentInChildren<Slider>();

        recordBtn.onClick.AddListener(PressedRecord);
        if (MuhanNpcDataManager.Instance == null)
        {
            gameMode = Define.GameMode.Story;
        }
        else
        {
            gameMode = Define.GameMode.Infinity;
        }
    }

    public void PressedRecord()
    {
        if (!_isRecording)
        {
            _isRecording = true;
            STTconnect.StartRecording();
        }
        else
        {
            _isRecording = false;
            STTconnect.StopRecording();
        }
    }

    public void OnRecordingOnUI()
    {
        SwitchInputFieldToSlider(false);
        SliderStart();
    }

    public void OnRecordingOffUI()
    {
        if (_isRecording) _isRecording = false;
        SwitchInputFieldToSlider();
    }

    public void SwitchInputFieldToSlider(bool isSliderToInput = true)
    {
        ChangeSprite();
        if (RecordInputRect != null)
        {
            RecordInputRect.SetActive(!isSliderToInput);
        }

        if (ResultInputfield != null)
        {
            ResultInputfield.gameObject.SetActive(isSliderToInput);
        }
    }
    public void SetSTTtxt(string playerSTT)//STT Connect에서 주입
    {
        if (playerSTT != null)
            ResultInputfield.GetComponent<TMP_InputField>().text = playerSTT;
        else
            ResultInputfield.GetComponent<TMP_InputField>().text = "";
    }

    public void NoMic()
    {
        noMicDetected.SetActive(true);
        _isRecording = false;
    }

    private void SliderStart()
    {
        _currentRecordingTime = 0f;
        recordSlider.maxValue = STTconnect._recordingLengthSec;
        recordSlider.value = STTconnect._recordingLengthSec;

        if (_recordingCoroutine != null)
        {
            StopCoroutine(_recordingCoroutine);
        }
        _recordingCoroutine = StartCoroutine(UpdateRecordingSlider());
    }

    private IEnumerator UpdateRecordingSlider()
    {
        while (_isRecording)
        {
            _currentRecordingTime += Time.deltaTime;  // 시간이 지남에 따라 증가
            recordSlider.value = STTconnect._recordingLengthSec - _currentRecordingTime; // 슬라이더 값 감소

            // 녹음 시간이 다 되면 녹음 중지
            if (recordSlider.value <= 0)
            {
                STTconnect.StopRecording();
                yield break;
            }
            yield return null;
        }
    }

    private void ChangeSprite()
    {
        if (gameMode == Define.GameMode.Story)
            recordImg.sprite = _isRecording ? recordOn : recordOff;
        else
        {
            recordImg.sprite = muhanRecordOn;
            if(!_isRecording)
                recordImg.color = Color.red;
        }       
    }
}
