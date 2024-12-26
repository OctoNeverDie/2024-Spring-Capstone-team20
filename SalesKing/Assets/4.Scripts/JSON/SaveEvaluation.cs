using System.Collections.Generic;
using UnityEngine;

public class SaveEvaluation : MonoBehaviour
{
    //특정 경로에 평가를 1일차마다 갱신 : id, isbuy, json화 해서 넣었다가 list로 빼오면 될 듯
    public struct EvaluationInfo
    {
        public int Day { get; private set; }
        public int NpcId { get; private set; }
        public bool IsBuy { get; private set; }
        public string Evaluation { get; private set; }
        public EvaluationInfo(int day, int npcId, bool isBuy, string evaluation)
        {
            Day = day;
            NpcId = npcId;
            IsBuy = isBuy;
            Evaluation = evaluation;
        }
    }

    public readonly List<EvaluationInfo> _allEvaluations = new List<EvaluationInfo>();
    private readonly List<EvaluationInfo> _todaysEvaluations = new List<EvaluationInfo>();

    private bool _isInitialized = false;
    private void InitializeIfNeeded()
    {
        if (_isInitialized) return;
        //그전에 txt 파일에 로드된 list 전부 가져오기
        // evaluationInfoList = txt 로드된 거
        _isInitialized = true;
    }

    public List<EvaluationInfo> GetAllEvaluations()//하루가 시작
    {
        InitializeIfNeeded();
        return _allEvaluations;
    }

    public void AddEvaluation(int day, int npcID, bool isBuy, string evaluation)//하루 진행
    {
        InitializeIfNeeded();
        _todaysEvaluations.Add(new EvaluationInfo(day, npcID, isBuy, evaluation));
    }

    public void SaveDailyEvaluations()//하루가 끝남
    {
        foreach (var eval in _todaysEvaluations)
        {
            //json화 해서 파일에 넣기
        }
        _allEvaluations.AddRange(_todaysEvaluations);
        _todaysEvaluations.Clear();
    }
}