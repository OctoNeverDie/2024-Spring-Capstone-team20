using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewsSpawner : Singleton<NewsSpawner>, ISingletonSettings
{
    [SerializeField] public DayEvalSO dayEvalSO;
    [SerializeField] private GameObject evalPanelSpawner;
    [SerializeField] private float waitforNext = 1;

    public NewsInfoInjector injector = new NewsInfoInjector();
    private PositionAdjuster adjuster = new PositionAdjuster();
    
    public int success { private set; get; } = 0;
    public int allEvent { private set; get; } = 0;

    public bool ShouldNotDestroyOnLoad => false;

    ////-----------------------------------
    //[Header("Test-----------------------")]
    //[SerializeField] private List<string> testSentences;
    //[SerializeField] private int testDay = 1;

    //private void OnEnable() {
    //    Test(testDay);
    //}

    //private void Test(int testDay) {
    //    foreach (var npc in testSentences) {
    //        injector.UpdateEvaluationData(npc);
    //    }

    //    ShowNews(testDay);
    //}
    ////-----------------------------------

    public void UpdateEvaluationData(string Evaluation, NpcInfo thisNpc, bool isBuy) {
        if(isBuy)
            success++;
        allEvent++;
        injector.UpdateEvaluationData(Evaluation, thisNpc);
    }

    public void ShowNews(int curDay) {
        SpawnItem(curDay);
        StartCoroutine(ShowInOrder());
    }

    private IEnumerator ShowInOrder() {
        for(int i =0; i<this.transform.childCount; i++) {
            yield return new WaitForSecondsRealtime(waitforNext);
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void SpawnItem(int curDay) {
        adjuster.InitSpacing(allEvent, GetComponentInParent<RectTransform>());

        RectTransform evalItem; 
        for (int i = 0; i <= allEvent; i++) {
            evalItem = InitItem(i);

            adjuster.AdjustPos(i, evalItem);

            if (i == allEvent) {
                string dayEval = injector.GetDayEval(curDay, success, dayEvalSO);
                injector.InjectInfo(dayEval, evalItem);
                break;
            }

            if (!injector.InjectInfo(i, evalItem)) {
                Destroy(evalItem);
            }
        }
    }

    private RectTransform InitItem(int i) {
        GameObject item = Instantiate(evalPanelSpawner, this.transform);
        item.SetActive(false);

        return item.GetComponentInChildren<RectTransform>();
    }
}
