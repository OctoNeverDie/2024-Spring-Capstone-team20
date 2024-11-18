using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TMP_InputField userInput; 
    [SerializeField] private Button EnterBtn;       

    private void Awake()
    {
        ReplySubManager.OnReplyUpdated += DeleteInput;

        EnterBtn.onClick.AddListener(UpdateInput);
    }

    private void Update()
    {
        if (Input.GetButtonDown("submit"))
        {
            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        // Ensure the input field is not empty
        if (!string.IsNullOrEmpty(userInput.text))
        {
            string reply = userInput.text.Trim(); // Get and trim the input text
            ChatManager.Instance.Reply.UserAnswer = reply; // Assign to ChatManager
        }
    }

    private void DeleteInput(string type, string _)//응답 오면 지워짐
    {
        if(type == nameof(ReplySubManager.GptAnswer))
            userInput.text = string.Empty;
    }
}

