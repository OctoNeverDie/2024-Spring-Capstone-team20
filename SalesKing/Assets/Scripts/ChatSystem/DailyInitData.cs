//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 무한 모드용. 지금은 짱박아두기
///// </summary>
//public class InitData
//{
//    public int npcID;
//    public int boughtItemID;

//    public Define.Prefer[] mbtiPrefers = new Define.Prefer[4];
//}

//public class DailyInitData : MonoBehaviour
//{
//    private int day = 0;
//    private List<InitData> initDatas = new List<InitData>();

//    public InitData getInitData() 
//    {
//        InitData initData = initDatas[0];
//        if (initDatas != null && initDatas.Count > 0)
//        {
//            // 첫 번째 요소 제거
//            initDatas.RemoveAt(0);
//        }
//        return initData;
//    }
    
//    private void Awake()
//    {
//        initMbtiQueue();
//        initNpcs();

//        for (int i = 0; i < 3; i++)
//            initDatas.Add(makeInitData(i));
//    }

//    #region init
//    private Queue<Define.Prefer[]> mbtiQueue;
//    private void initMbtiQueue()
//    {
//        mbtiQueue = new Queue<Define.Prefer[]>();

//        if (day == 2)
//        {
//            // 큐에 배열들을 푸시
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.like, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm });
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.like, Define.Prefer.norm, Define.Prefer.norm });
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.dislike, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm });
//        }
//        else if (day == 3)
//        {
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.like, Define.Prefer.norm });
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.like });
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.dislike, Define.Prefer.norm });
//        }
//        else if (day == 4)
//        {
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.dislike });
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.dislike, Define.Prefer.like, Define.Prefer.norm, Define.Prefer.norm });
//            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.like, Define.Prefer.dislike });
//        }
//    }

//    private List<int> npcList = new List<int>();
//    private void initNpcs()
//    {
//        HashSet<int> npcSet = new HashSet<int>();

//        while (npcSet.Count < 3)
//        {
//            int randomIndex = Random.Range(0, Managers.Data.npcList.Count);
//            npcSet.Add(randomIndex);
//        }

//        npcList = new List<int>(npcSet);
//    }
//    #endregion
//    private InitData makeInitData(int i)
//    {
//        InitData initData = new InitData();

//        initData.npcID = npcList[i];
//        initData.mbtiPrefers = getMbtis();

//        return initData;
//    }

//    private Define.Prefer[] getMbtis()
//    {
//        if (mbtiQueue.Count > 0)
//        {
//            return mbtiQueue.Dequeue();
//        }

//        Debug.Log("엠벼 다 씀");
//        return null;
//    }


//    private int getItems(int wantItemID)
//    {
//        int idx;
//        do
//        {
//            idx = UnityEngine.Random.Range(0, Managers.Data.itemList.Count);
//        } while (wantItemID == idx);
       
//        return idx;
//    }
//}