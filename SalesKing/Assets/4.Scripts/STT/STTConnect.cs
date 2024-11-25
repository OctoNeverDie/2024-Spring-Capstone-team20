using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class STTConnect : MonoBehaviour
{
    public static event Action<bool> OnSendClovaUpdate;

    public string _microphoneID = null;
    public int _recordingLengthSec = 15;

    private int _recordingHZ = 22050;
    private AudioClip _recording = null;
    
    // Naver Clova API URL 및 인증 키
    private string apiURL = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=Kor";
    private string apiKeyID = Environment.GetEnvironmentVariable("X_NCP_APIGW_API_KEY_ID", EnvironmentVariableTarget.User);
    private string apiKey = Environment.GetEnvironmentVariable("X_NCP_APIGW_API_KEY", EnvironmentVariableTarget.User);

    private RecordInput recordInputUI;

    private void Awake()
    {
        recordInputUI = this.GetComponent<RecordInput>();

        if (Microphone.devices.Length > 0)
        {
            _microphoneID = Microphone.devices[0];
        }
    }

    public void StartRecording()
    {
        if (_microphoneID == null)
        {
            recordInputUI.NoMic();
            return;
        }

        Debug.Log("녹음 시작");
        recordInputUI.OnRecordingOnUI();

        _recording = Microphone.Start(_microphoneID, true, _recordingLengthSec, _recordingHZ);
    }

    public void StopRecording()//15초 다 됐거나, 그 전에 input 누르거나
    {
        if (Microphone.IsRecording(_microphoneID))
        {
            Debug.Log("녹음 종료");
            recordInputUI.OnRecordingOffUI();
            Microphone.End(_microphoneID);
            
            if (_recording == null)
            {
                Debug.LogError("녹음된 오디오가 없습니다.");
                return;
            }

            SendToClova();
        }
    }

    private void SendToClova()
    {
        OnSendClovaUpdate.Invoke(true);
        // AudioClip을 WAV 형식의 바이트 배열로 변환
        byte[] byteData = GetWavBytesFromAudioClip(_recording);
        // API 서버로 오디오 데이터 전송
        StartCoroutine(PostVoiceToAPI(byteData));
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

            recordInputUI.SetSTTtxt(voiceRecognize.text);
            OnSendClovaUpdate.Invoke(false);
        }
    }

    // STT 결과를 받기 위한 클래스
    [System.Serializable]
    public class VoiceRecognize
    {
        public string text;
    }
}
