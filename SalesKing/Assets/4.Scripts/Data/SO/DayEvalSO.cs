using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayEvalSO", menuName = "SO/DayEvalSO")]
public class DayEvalSO : ScriptableObject
{
    [System.Serializable]
    public class DayEval {
        public int Day;
        public string GoodSentence;
        public string BadSentence;

        public DayEval(int day, string good, string bad) { 
            this.Day = day;
            GoodSentence = good;
            BadSentence = bad;
        }
    }

    public List<DayEval> daySentences = new List<DayEval>();
}
