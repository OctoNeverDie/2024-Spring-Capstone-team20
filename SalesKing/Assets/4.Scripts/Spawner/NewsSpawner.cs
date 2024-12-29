using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewsSpawner : Singleton<NewsSpawner>, ISingletonSettings
{
    [SerializeField] private GameObject backPanel;
    [SerializeField] public DayEvalSO dayEvalSO;
    [SerializeField] private GameObject evalPanelSpawner;
    [SerializeField] private float waitforNext = 1;

    public NewsInfoInjector injector = new NewsInfoInjector();
    private PositionAdjuster adjuster = new PositionAdjuster();
    
    public int success { private set; get; } = 0;
    public int allEvent { private set; get; } = 0;

    public bool ShouldNotDestroyOnLoad => false;

    ////-----------------------------------
    [Header("Test-----------------------")]
    [SerializeField] private List<string> testSentences;
    [SerializeField] private int testDay = 1;

    private void OnEnable()
    {
        Test(testDay);
    }

    private void Test(int testDay)
    {
        foreach (var npc in testSentences)
        {
            allEvent++;
            injector.UpdateEvaluationData(npc);
        }
        
        ShowNews(testDay);
    }
    ////-----------------------------------

    public void UpdateEvaluationData(string Evaluation, NpcInfo thisNpc, bool isBuy) {
        if(isBuy)
            success++;
        allEvent++;

        if (thisNpc.NpcID == 1) { //제리체리제리
            DataController.Instance.gameData.jerryCherry_1_3 = Evaluation;
        }

        injector.UpdateEvaluationData(Evaluation, thisNpc);
    }

    public void ShowNews(int curDay) {
        backPanel.SetActive(true);
        SpawnItem(curDay);
        StartCoroutine(ShowInOrder());
    }

    private IEnumerator ShowInOrder() {
        for(int i =0; i<this.transform.childCount; i++) {
            yield return new WaitForSecondsRealtime(waitforNext);
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void SpawnItem(int curDay)
    {
        List<RectTransform> itemList = new List<RectTransform>();

        for (int i = 0; i <= allEvent; i++)
        {
            RectTransform evalItem = InitItem(i);

            if (i == allEvent)
            {//마지막 뉴스
                string dayEval = injector.GetDayEval(curDay, success, dayEvalSO);
                injector.InjectInfo(dayEval, evalItem);
                itemList.Add(evalItem);
                break;
            }

            if (!injector.InjectInfo(i, evalItem))//텍스트가 공백이라면 destroy
                Destroy(evalItem.gameObject);
            else
                itemList.Add(evalItem);
        }

        adjuster.InitSpacing(itemList.Count, GetComponentInParent<RectTransform>());
        for (int i = 0; i < itemList.Count; i++)
        {
            RectTransform item = itemList[i];
            adjuster.AdjustPos(i, item);
        }
    }

    private RectTransform InitItem(int i) {//뉴스 obj 담는 거 스폰
        GameObject item = Instantiate(evalPanelSpawner, this.transform);
        item.SetActive(false);

        return item.GetComponentInChildren<RectTransform>();//실제 뉴스 obj 리턴
    }
}
