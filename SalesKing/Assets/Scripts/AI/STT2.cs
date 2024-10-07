using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using OpenAI_API;
using UnityEngine.UI;

public class STT2 : MonoBehaviour
{
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingLengthSec = 15;
    private int _recordingHZ = 22050;

    public TMP_InputField myInputField;
    public Slider recordingSlider;  // 슬라이더를 public으로 받아옴

    private float currentRecordingTime = 0f;
    private Coroutine recordingCoroutine;

    // Naver Clova API URL 및 인증 키
    private string apiURL = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=Kor";
    private string apiKeyID;
    private string apiKey;

    void Start()
    {
        // 마이크 초기화
        if (Microphone.devices.Length > 0)
        {
            _microphoneID = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("마이크 장치를 찾을 수 없습니다.");
        }

        // 요청 헤더 설정
        apiKeyID = Environment.GetEnvironmentVariable("X_NCP_APIGW_API_KEY_ID", EnvironmentVariableTarget.User);
        apiKey = Environment.GetEnvironmentVariable("X_NCP_APIGW_API_KEY", EnvironmentVariableTarget.User);

        recordingSlider.maxValue = _recordingLengthSec; // 슬라이더 최대값을 녹음 시간으로 설정
        recordingSlider.value = _recordingLengthSec;
    }

    void Update()
    {
        if (Input.GetButtonDown("STT"))
        {
            StartRecording();
        }
        if (Input.GetButtonUp("STT"))
        {
            StopRecording();
        }
    }

    public void StartRecording()
    {
        if (_microphoneID == null)
        {
            Debug.LogError("마이크 ID가 설정되지 않았습니다.");
            return;
        }
        Debug.Log("녹음 시작");
        _recording = Microphone.Start(_microphoneID, true, _recordingLengthSec, _recordingHZ);

        UpdateRecordingSlider();

        currentRecordingTime = 0f;
        recordingSlider.value = _recordingLengthSec;  // 슬라이더를 다시 채움

        // Coroutine으로 슬라이더 업데이트 시작
        if (recordingCoroutine != null)
        {
            StopCoroutine(recordingCoroutine);
        }
        recordingCoroutine = StartCoroutine(UpdateRecordingSlider());
    }

    public void StopRecording()
    {
        if (Microphone.IsRecording(_microphoneID))
        {
            recordingSlider.value = _recordingLengthSec;

            Microphone.End(_microphoneID);
            Debug.Log("녹음 종료");

            if (_recording == null)
            {
                Debug.LogError("녹음된 오디오가 없습니다.");
                return;
            }

            // AudioClip을 WAV 형식의 바이트 배열로 변환
            byte[] byteData = GetWavBytesFromAudioClip(_recording);

            // API 서버로 오디오 데이터 전송
            StartCoroutine(PostVoiceToAPI(byteData));
        }
    }

    private IEnumerator UpdateRecordingSlider()
    {
        while (Microphone.IsRecording(_microphoneID))
        {
            currentRecordingTime += Time.fixedDeltaTime;  // 시간이 지남에 따라 증가
            recordingSlider.value = _recordingLengthSec - currentRecordingTime; // 슬라이더 값 감소

            // 녹음 시간이 다 되면 녹음 중지
            if (currentRecordingTime >= _recordingLengthSec)
            {
                StopRecording();
                yield break;
            }

            yield return null;  // 한 프레임 대기
        }
    }

    // AudioClip을 WAV 바이트 배열로 변환하는 함수
    private byte[] GetWavBytesFromAudioClip(AudioClip clip)
    {
        // 필요한 경우 WAV 변환 유틸리티를 사용해야 합니다.
        // 여기서는 간단히 예시를 제공합니다.
        // 실제로는 AudioClip을 WAV 포맷으로 변환하는 기능을 구현해야 합니다.
        return WavUtility.FromAudioClip(clip);
    }

    private IEnumerator PostVoiceToAPI(byte[] byteData)
    {
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(byteData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", apiKeyID);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // JSON 파싱하여 결과 출력
            VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(responseText);
            myInputField.text += voiceRecognize.text;
        }
    }

    // STT 결과를 받기 위한 클래스
    [System.Serializable]
    public class VoiceRecognize
    {
        public string text;
    }
}
