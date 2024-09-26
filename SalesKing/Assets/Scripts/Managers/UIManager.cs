using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Newtonsoft.Json.Bson;

public class UIManager : MonoBehaviour
{
    GameObject myCanvas;
    ConvoUI ui;

    void Start()
    {
        myCanvas = GameObject.Find("Canvas");
        ui = myCanvas.GetComponent<ConvoUI>();
        SetTimeText();
    }

    private void Update()
    {
        SetTimeText();

        // stt 시작 버튼
        if (Input.GetButtonDown("STT"))
        {
            Debug.Log("stt 시작");
        }

        // stt 종료 버튼
        if (Input.GetButtonUp("STT"))
        {
            Debug.Log("stt 끝");
        }

        // 제출 버튼
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("대화 보내기");
        }
    }

    public void ShowTalkOrNotPanel()
    {
        ui.TalkOrNotPanel.SetActive(true);
    }

    public void SetNPCAnswerText(string text)
    {
        ui.NPCSpeechText.text = text;
        ui.NPCSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ui.NPCSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);
    }

    public void SetStatusText(string thought, string reason, string emotion, string suggestprice)
    {
        ui.StatusText.text = "Thought: "+thought+"\n"
            +"Reason: " + reason + "\n"
            +"Emotion: " + emotion + "\n"
            +"Suggested Price: " + suggestprice + "\n";
    }

    public void SetTimeText()
    {
        int hour = Managers.Time.Hour;
        int minute = Managers.Time.Minute;

        string hour_s;
        string min_s;
        string ampm;

        if(hour < 10) hour_s = "0"+hour;
        else hour_s = hour.ToString();

        if (minute < 10) min_s = "00";
        else min_s = minute/10+"0";

        if (hour < 12) ampm = "AM";
        else ampm = "PM";

        ui.TimeText.text = hour_s+" : "+min_s+" "+ampm;
    }

    public void SetPlayerInputField(string input)
    {
        ui.UserText.text = ui.UserText.text + input;
    }

}
