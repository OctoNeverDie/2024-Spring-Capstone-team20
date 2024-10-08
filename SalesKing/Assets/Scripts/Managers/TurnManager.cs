using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // 오늘 몇일차인지
    public int todayDate;
    // 오늘 몇번 대화했는지
    public int todayTurn=0;
    // 대화 횟수 제한
    public int talkLimit = 3;

    public TextMeshProUGUI todayCashText;
    public GameObject SummaryPanel;

    private float todayCashFloat;

    private float todayGoal = 200;

    void Start()
    {
        Managers.UI.SetTurnText(todayTurn, talkLimit);
    }

    public void AddTurnAndCheckTalkTurn()
    {
        todayTurn++;
        Managers.UI.SetTurnText(todayTurn, talkLimit);
        if (todayTurn >= talkLimit)
        {
            Debug.Log("대화 횟수 소진, 엔딩 호출 (위치: TurnManager)");
            ShowSummary();

        }
    }

    public void ShowSummary()
    {
        SummaryPanel.SetActive(true);
        todayCashFloat = Managers.Cash.TotalCash;
        todayCashText.text = "오늘 번 돈: " + Managers.Cash.TotalCash.ToString();
    }

    public void EndingScene()
    {
        if(todayCashFloat < todayGoal)
        {
            Debug.Log("성공~");
        } else
        {
            Debug.Log("실패~");
        }
    }

}
