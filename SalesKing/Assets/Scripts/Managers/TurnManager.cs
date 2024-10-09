using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // 오늘 몇일차인지
    public int todayDate;
    // 오늘 몇번 대화했는지
    public int todayTurn=0;
    // 대화 횟수 제한
    public int talkLimit = 3;

    private float todayCashFloat;

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
            Managers.UI.ShowSummaryPanel();
        }
    }

}
