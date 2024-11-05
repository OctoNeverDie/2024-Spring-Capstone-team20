using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_old : MonoBehaviour
{
    GameObject myCanvas;
    public ConvoUI ui;

    void Start()
    {
        myCanvas = GameObject.Find("Canvas");
        ui = myCanvas.GetComponent<ConvoUI>();
        SetTimeText();
    }

    private void Update()
    {
        SetTimeText();
    }

    public void ShowTalkOrNotPanel()
    {
        ui.TalkOrNotPanel.SetActive(true);
    }

    public void ShowSummaryPanel()
    {
        ui.todayCashText.text = "오늘 번 돈: " + Managers.Cash.TotalCash.ToString() + "$";
        ui.SummaryPanel.SetActive(true);
    }

    public void SetTimeText()
    {
        int hour = Managers.Time.Hour;
        int minute = Managers.Time.Minute;

        string hour_s;
        string min_s;
        string ampm;

        if (hour < 10) hour_s = "0" + hour;
        else hour_s = hour.ToString();

        if (minute < 10) min_s = "00";
        else min_s = minute / 10 + "0";

        if (hour < 12) ampm = "AM";
        else ampm = "PM";

        ui.TimeText.text = hour_s + " : " + min_s + " " + ampm;
    }

    public void SetCashText(string cashAmount)
    {
        ui.CashText.text = cashAmount + "$";
    }

    public void SetTurnText(int turn, int maxTurn)
    {
        ui.TurnText.text = turn.ToString() + " / " + maxTurn.ToString();
    }

    public void InitiateInputMode()
    {
        Define.UserInputMode defaultMode = Managers.Input.CurInputMode;

        if (defaultMode == Define.UserInputMode.Keyboard)
        {
            Debug.Log("키보드 인풋 모드로 초기화");
            if (ui != null)
            {
                ui.RecordPanel.SetActive(false);
                ui.KeyboardPanel.SetActive(true);
            }
        }
        else if (defaultMode == Define.UserInputMode.Voice)
        {
            ui.RecordPanel.SetActive(true);
            ui.KeyboardPanel.SetActive(false);
        }
    }
}
