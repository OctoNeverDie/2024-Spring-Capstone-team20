using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject myCanvas;
    ConvoUI ui;

    void Start()
    {
        myCanvas = GameObject.Find("Canvas");
        ui = myCanvas.GetComponent<ConvoUI>();
        SetTimeText();
        SetTurnText();
    }

    private void Update()
    {
        SetTimeText();
    }

    public void ShowTalkOrNotPanel()
    {
        ui.TalkOrNotPanel.SetActive(true);
    }

    public void UpdateConvoPanel()
    {

    }

    public void InitiateConvoPanel()
    {

    }

    public void SetNPCAnswerText(string text)
    {
        ui.NPCSpeechText.text = text;
    }

    public void SetStatusText(int suggestedPrice, int persuasion)
    {
        ui.StatusText.text = "Suggested Price: "+suggestedPrice+"\nPersuasion: "+persuasion;
    }

    public void SetTimeText()
    {
        int hour = Managers.Turn.Hour;
        int minute = Managers.Turn.Minute;

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

    public void SetTurnText()
    {
        ui.TurnText.text = Managers.Turn.curTurn+" / 3";
    }

}
