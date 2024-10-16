using System.Collections;
using UnityEngine;

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
            Debug.Log("TODO : 버튼 나오게 해야함, 대화 횟수 소진, 엔딩 호출 (위치: TurnManager)");
            StartCoroutine(ShowSummaryPanelWithDelay(1f));
        }
    }
    private IEnumerator ShowSummaryPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Managers.UI.ShowSummaryPanel();
    }
}
