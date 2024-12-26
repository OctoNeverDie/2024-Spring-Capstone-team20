using UnityEngine;
using System.Collections;

public class NewsSpawner : MonoBehaviour
{
    [SerializeField] public DayEvalSO dayEvalSO;
    [SerializeField] private GameObject evalPanelSpawner;
    [SerializeField] private float waitforNext = 1;

    private NewsInfoInjector injector = new NewsInfoInjector();
    private PositionAdjuster adjuster = new PositionAdjuster();
    private int success = 0 ;

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

    public void UpdateEvaluationData(string Evaluation, NpcInfo thisNpc) {
        success++;
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
        adjuster.InitSpacing(success, GetComponentInParent<RectTransform>());

        RectTransform evalItem; 
        for (int i = 0; i <= success; i++) {
            evalItem = InitItem(i);

            adjuster.AdjustPos(i, evalItem);

            if (i == success) {
                string dayEval = injector.GetDayEval(curDay, success, dayEvalSO);
                injector.InjectInfo(dayEval, evalItem);
                break;
            }

            injector.InjectInfo(i, evalItem);
        }
    }

    private RectTransform InitItem(int i) {
        GameObject item = Instantiate(evalPanelSpawner, this.transform);
        item.SetActive(false);

        return item.GetComponentInChildren<RectTransform>();
    }
}
