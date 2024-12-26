using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NewsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject evalPanelSpawner;
    [SerializeField] private DayEvalSO dayEvalSO;
    [SerializeField] private float waitforNext = 1;
    [Header("Test-----------------------")]
    [SerializeField] private List<string> testSentences;
    [SerializeField] private int testDay = 1;

    private void OnEnable() {
        Test(testDay);
    }

    private void Test(int testDay) {
        foreach (var npc in testSentences) {
            UpdateEvaluationData(npc);
        }

        ShowNews(testDay);
    }

    [System.Serializable]
    private class NpcEval
    {
        public NpcInfo Npc { get; private set; }
        public string News { get; private set; }

        public NpcEval(NpcInfo _npcID, string _news)
        {
            this.Npc = _npcID;
            this.News = _news;
        }
    }

    private List<NpcEval> NpcNews = new List<NpcEval>();

    public void UpdateEvaluationData(string summary, NpcInfo thisNpc = null) {
        NpcNews.Add(new NpcEval(thisNpc, summary));
    }

    public void ShowNews(int curDay) {
        InitSpacing();
        SpawnItem(curDay);
        StartCoroutine(ShowInOrder());
    }

    private IEnumerator ShowInOrder() {
        for(int i =0; i<this.transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(waitforNext);
        }
    }

    private void InitSpacing()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float spacing = rt.rect.height / (NpcNews.Count +2);

        VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();
        layout.spacing = spacing;
    }

    private void SpawnItem(int curDay) {
        Transform evalItem; 
        for (int i = 0; i <= NpcNews.Count; i++) {
            evalItem = InitItem(i);

            AdjustPos(i, evalItem);

            if (i == NpcNews.Count) {
                string dayEval = GetDayEval(curDay);
                InjectInfo(dayEval, evalItem);
                break;
            }

            InjectInfo(i, evalItem);
        }
    }

    private Transform InitItem(int i) {
        Transform evalItem = Instantiate(evalPanelSpawner, this.transform).transform;
        evalItem.gameObject.SetActive(false);
        
        return evalItem.GetChild(0);
    }

    private readonly List<(float PositionX, float Rotation)> positionData = new List<(float PositionX, float Rotation)> {
        (-26f, 6f),
        (28f, -2.4f)
    };

    private void AdjustPos(int alignmentKey, Transform item) {
        int index = (alignmentKey % 2 == 0) ? 0 : 1;

        //position
        item.position = new Vector3(
            item.position.x + positionData[index].PositionX + Random.Range(-5.0f, 5.0f),
            item.position.y,
            item.position.z
        );

        //rotation
        float adjustRotation = positionData[index].Rotation + Random.Range( -2.0f, 2.0f );
        item.rotation = Quaternion.Euler(0, 0, adjustRotation);
    }

    private void InjectInfo(int i, Transform item) {
        TextMeshProUGUI news = item.GetComponentInChildren<TextMeshProUGUI>();
        news.text = NpcNews[i].News;
    }
    private void InjectInfo(string eval, Transform item) {
        TextMeshProUGUI news = item.GetComponentInChildren<TextMeshProUGUI>();
        news.text = eval;
    }

    private string GetDayEval(int curDay) {//1일차
        var dayEval = dayEvalSO.daySentences[curDay-1];//0번 인덱스
        string evalText = "";
        
        switch (NpcNews.Count) {
            case 1:
            case 2:
                evalText = dayEval.BadSentence;
                break;
            case 3:
            case 4:
                evalText = dayEval.GoodSentence;
                break;
            default:
                evalText = dayEval.BadSentence;
                break;
        }

        return evalText;
    }
}
