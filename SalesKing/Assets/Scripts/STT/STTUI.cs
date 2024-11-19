using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class STTUI : MonoBehaviour
{
    STT2 stt;
    public TMP_InputField myInputField;
    public Slider recordingSlider;  // 슬라이더를 public으로 받아옴

    private float currentRecordingTime = 0f;
    private Coroutine recordingCoroutine;

    private bool isFixingRecord = false;

    // Start is called before the first frame update
    void Start()
    {
        stt = GetComponent<STT2>();
        if (recordingSlider != null)
        {
            recordingSlider.maxValue = stt._recordingLengthSec; // 슬라이더 최대값을 녹음 시간으로 설정
            recordingSlider.value = stt._recordingLengthSec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (UserInputManager.Instance.CurInputMode == Define.UserInputMode.Voice && !isFixingRecord)
        //{
        //    Debug.Log("cur mode is " + UserInputManager.Instance.CurInputMode);
        //    if (Input.GetButtonDown("STT"))
        //    {
        //        stt.StartRecording();
        //    }
        //    if (Input.GetButtonUp("STT"))
        //    {
        //        stt.StopRecording();
        //    }
        //}
    }

    public void StartRecordingUI()
    {
        UpdateRecordingSlider();

        currentRecordingTime = 0f;
        recordingSlider.value = stt._recordingLengthSec;  // 슬라이더를 다시 채움

        // Coroutine으로 슬라이더 업데이트 시작
        if (recordingCoroutine != null)
        {
            StopCoroutine(recordingCoroutine);
        }
        recordingCoroutine = StartCoroutine(UpdateRecordingSlider());
    }

    public void StopRecordingUI()
    {
        recordingSlider.value = stt._recordingLengthSec;
        recordingSlider.gameObject.SetActive(false);
        myInputField.gameObject.SetActive(true);
        isFixingRecord = true;
        ClearSTTText();
    }

    private IEnumerator UpdateRecordingSlider()
    {
        while (Microphone.IsRecording(stt._microphoneID))
        {
            currentRecordingTime += Time.fixedDeltaTime;  // 시간이 지남에 따라 증가
            recordingSlider.value = stt._recordingLengthSec - currentRecordingTime; // 슬라이더 값 감소

            // 녹음 시간이 다 되면 녹음 중지
            if (currentRecordingTime >= stt._recordingLengthSec)
            {
                stt.StopRecording();
                yield break;
            }

            yield return null;  // 한 프레임 대기
        }
    }

    public void SetSTTText(string newText)
    {
        myInputField.text += newText;

    }

    public void ClearSTTText()
    {
        myInputField.text = null;

    }

    public void OnClickEnter()
    {
        recordingSlider.gameObject.SetActive(true);
        myInputField.gameObject.SetActive(false);
        isFixingRecord = false;
    }
}
