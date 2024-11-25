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
    [SerializeField] TMP_InputField ResultInputfield;
    [SerializeField] RectTransform RecordInputRect;//contains button, button image, slider
    [SerializeField] Sprite recordOn;
    [SerializeField] Sprite recordOff;
    [SerializeField] GameObject noMicDetected;

    Image recordImg;
    Button recordBtn;
    Slider recordSlider;

    STTConnect STTconnect;

    private bool _isRecording = false;
    private float _currentRecordingTime;
    private Coroutine _recordingCoroutine;

    private void Awake()
    {
        STTconnect = this.GetComponent<STTConnect>();

        recordImg = RecordInputRect.GetComponent<Image>();
        recordBtn = RecordInputRect.GetComponent<Button>();
        recordSlider = RecordInputRect.GetComponentInChildren<Slider>();

        recordBtn.onClick.AddListener(PressedRecord);
    }

    private void Update()
    {
        if (Input.GetButtonDown("STT"))
            PressedRecord();
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
        ChangeSprite();
        RecordInputRect.gameObject.SetActive(true);
        ResultInputfield.gameObject.SetActive(false);

        SliderStart();
    }

    public void OnRecordingOffUI()
    {
        ChangeSprite();
        RecordInputRect.gameObject.SetActive(false);
        ResultInputfield.gameObject.SetActive(true);
    }

    public void SetSTTtxt(string playerSTT)//STT Connect에서 주입
    {
        ResultInputfield.text = playerSTT;
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
        while (Microphone.IsRecording(STTconnect._microphoneID) && _isRecording)
        {
            _currentRecordingTime += Time.fixedDeltaTime;  // 시간이 지남에 따라 증가
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
        recordImg.sprite = _isRecording ? recordOn : recordOff;
    }
}
