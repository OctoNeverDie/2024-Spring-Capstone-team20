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

    private List<NpcEval> NpcNews = new List<NpcEval>();

    public void UpdateEvaluationData(string summary, NpcInfo thisNpc = null)
    {
        NpcNews.Add(new NpcEval(thisNpc, summary));
    }

    public void InjectInfo(int i, RectTransform item)
    {
        TextMeshProUGUI news = item.GetComponentInChildren<TextMeshProUGUI>();
        news.text = NpcNews[i].News;
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