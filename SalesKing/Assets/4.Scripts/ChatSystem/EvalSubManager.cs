using System.Collections.Generic;
using UnityEngine;

public class EvalSubManager
{
    public class NpcEvaluation
    { //마지막에, 이름, MBTI 타입, 나이, 성별, 키워드, 사고 싶은 물건, 판 물건, 평가
        public int npcID;
        public int boughtItemID;

        public bool isSuccess;
        public string reason;
        public string summary;
    }
    public Dictionary<int, NpcEvaluation> NpcEvalDict { get; private set; } = new Dictionary<int, NpcEvaluation>();

    public int currentNpcId = 0;
    public NpcEvaluation _npcEvaluation;

    public void InitEvalDictNpc(int npcID, int playerItemIdx)
    {
        _npcEvaluation = new NpcEvaluation();
        _npcEvaluation.boughtItemID = playerItemIdx;
        _npcEvaluation.npcID = npcID;
        currentNpcId = _npcEvaluation.npcID;

        if (!NpcEvalDict.ContainsKey(npcID))
        {
            NpcEvalDict.Add(npcID, _npcEvaluation);
        }
        else
        { 
            Debug.Log("중복된 캐릭터 ID");
            NpcEvalDict[npcID] = _npcEvaluation;
        }        
    }

    public void AddEvaluation(string summary, bool isBuy)
    {
        NpcEvalDict[currentNpcId].summary = summary;
        NpcEvalDict[currentNpcId].isSuccess = isBuy;
    }
}
