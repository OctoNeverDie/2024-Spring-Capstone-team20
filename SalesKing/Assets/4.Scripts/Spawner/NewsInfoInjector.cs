using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsInfoInjector 
{
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
    //임시. 원래는 json 형태로 넣어놓고, dictionary로 변환해야함. 만약 새로하기 한다면 삭제하고.
    public readonly static Dictionary<int, string> allNews = new Dictionary<int, string>();
    private List<NpcEval> TodayNpcNews = new List<NpcEval>();

    public void UpdateEvaluationData(string summary, NpcInfo thisNpc = null)
    {
        TodayNpcNews.Add(new NpcEval(thisNpc, summary));

        if(allNews.ContainsKey(thisNpc.NpcID))
            allNews[thisNpc.NpcID] = summary;
        else
            allNews.Add(thisNpc.NpcID, summary);
    }

    public bool InjectInfo(int i, RectTransform item)
    {
        TextMeshProUGUI news = item.GetComponentInChildren<TextMeshProUGUI>();
        if (TodayNpcNews[i].News.Trim() != "") {
            news.text = TodayNpcNews[i].News;
            return true;
        }
        return false;
    }
    public void InjectInfo(string eval, Transform item)
    {
        TextMeshProUGUI news = item.GetComponentInChildren<TextMeshProUGUI>();
        news.text = eval;
    }

    public string GetDayEval(int curDay, int success, DayEvalSO dayEvalSO)
    {//1일차
        var dayEval = dayEvalSO.daySentences[curDay];//0번 인덱스
        string evalText = "";

        switch (success)
        {
            case 0:
            case 1:
                evalText = dayEval.BadSentence;
                break;
            case 2:
            case 3:
                evalText = dayEval.GoodSentence;
                break;
            default:
                evalText = dayEval.BadSentence;
                break;
        }

        return evalText;
    }

}