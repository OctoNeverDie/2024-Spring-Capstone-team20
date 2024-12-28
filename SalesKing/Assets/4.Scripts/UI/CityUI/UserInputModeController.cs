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
    [SerializeField] private Button EnterBtn;

    public Define.UserInputMode inputMode = Define.UserInputMode.Keyboard;
    private TMP_InputField userInput;
    private bool isSending = true;

    private void Awake()
    {
        ReplySubManager.OnReplyUpdated -= DeleteInput;
        ReplySubManager.OnReplyUpdated += DeleteInput;

        EnterBtn.onClick.AddListener(UpdateInput);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && !isSending) {
            DeleteLastLetter(userKeyboard, '\n');
            UpdateInput();
        }//Enter

        if (Input.GetButtonDown("Tab")) {
            DeleteLastLetter(userKeyboard, '\t');
        }
    }

    private void UpdateInput()
    {
        userInput = userKeyboard;
        userInput.text = userInput.text.Trim();
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
            isSending = false;
        }
    }

    private void DeleteLastLetter (TMP_InputField inputField, char letter)
    {
        if (inputField == null) return;

        if (inputField.text.Length > 0 && inputField.text[inputField.text.Length - 1] == letter)
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

}

