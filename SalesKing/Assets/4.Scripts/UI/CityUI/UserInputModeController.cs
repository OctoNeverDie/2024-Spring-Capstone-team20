using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInputMode : MonoBehaviour
{
    //record 모드일 때
    //space 눌렀을 시 녹음 시작
    //space 다시 누르면 녹음 완료
    //슬라이더가 panel로 바뀌고 거기에 user reply 나옴 + wait for reply 나옴
    //서버에 전송
    //도착 후 npc reply 나옴
    //1초 뒤 슬라이더로 바뀜
    [SerializeField] private TMP_InputField userKeyboard;
    [SerializeField] private TMP_InputField userVoice;
    [SerializeField] private Button EnterBtn;
    [SerializeField] private Button VoicetoTxtBtn;
    [SerializeField] private Button TxttoVoiceBtn;

    public Define.UserInputMode inputMode = Define.UserInputMode.Voice;
    private TMP_InputField userInput;
    private RecordInput recordInput;
    private bool isSending = true;

    private void Awake()
    {
        ReplySubManager.OnReplyUpdated -= DeleteInput;
        ReplySubManager.OnReplyUpdated += DeleteInput;

        EnterBtn.onClick.AddListener(UpdateInput);
        VoicetoTxtBtn.onClick.AddListener(() => VoiceToTxt(true));
        TxttoVoiceBtn.onClick.AddListener(() => VoiceToTxt(false));

        recordInput = GetComponent<RecordInput>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && !isSending)//Enter
        { 
            if (inputMode == Define.UserInputMode.Voice && !recordInput._isRecording)
                UpdateInput();
            else if(inputMode == Define.UserInputMode.Keyboard)
                UpdateInput();
        }

        if (Input.GetButtonDown("STT") && inputMode == Define.UserInputMode.Voice && !isSending && ChatManager.Instance.isConvo)//Space bar
            recordInput.PressedRecord();

        if (Input.GetButtonDown("Tab"))
        {
            ToggleInputField(userVoice);
            ToggleInputField(userKeyboard);
        }
    }

    private void UpdateInput()
    {
        userInput = (inputMode == Define.UserInputMode.Voice) ? userVoice : userKeyboard;
        
        if (!string.IsNullOrEmpty(userInput.text))
        {
            string reply = userInput.text.Trim(); // Get and trim the input text
            ChatManager.Instance.Reply.UserAnswer = reply; // Assign to ChatManager
            isSending = true;
        }
    }

    private void DeleteInput(string type, string _)//응답 오면 지워짐
    {
        if (type == nameof(ReplySubManager.GptAnswer))
        {
            if (userInput != null)
            {
                userInput.text = "";
                userInput.ActivateInputField();
            } 
            recordInput.SwitchInputFieldToSlider(false);
            isSending = false;
        }
    }

    public void VoiceToTxt(bool isVoicetotxt)
    {
        inputMode = isVoicetotxt ? Define.UserInputMode.Keyboard : Define.UserInputMode.Voice;
    }


    private void ToggleInputField(TMP_InputField inputField)
    {
        if (inputField == null) return;

        if (inputField.isFocused)
            inputField.DeactivateInputField();
    }

}

