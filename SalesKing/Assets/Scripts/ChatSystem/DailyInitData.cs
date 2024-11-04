using System.Collections.Generic;
using UnityEngine;

public class InitData
{
    public int npcID;
    public int itemID;
    public Define.Prefer[] mbtiPrefers = new Define.Prefer[4];
}

public class DailyInitData : MonoBehaviour
{
    private int day = 0;
    private List<InitData> initDatas = new List<InitData>();
    private List<int> npcList = new List<int>();

    public void getInitData()
    { 

    }
    
    private void Awake()
    {
        initMbtiQueue();

        for (int i = 0; i < 3; i++)
            initDatas.Add(makeInitData(i));
    }

    private InitData makeInitData(int i)
    {
        InitData initData = new InitData();

        initData.mbtiPrefers = getMbtis();
        initData.itemID = getItems();
        initData.npcID = npcList[i];

        return initData;
    }

    private Queue<Define.Prefer[]> mbtiQueue;
    private void initMbtiQueue()
    {
        mbtiQueue = new Queue<Define.Prefer[]>();

        if (day == 2)
        {
            // 큐에 배열들을 푸시
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.like, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm });
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.like, Define.Prefer.norm, Define.Prefer.norm });
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.dislike, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm });
        }
        else if (day == 3)
        {
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.like, Define.Prefer.norm });
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.like });
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.dislike, Define.Prefer.norm });
        }
        else if (day == 4)
        {
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.dislike });
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.dislike, Define.Prefer.like, Define.Prefer.norm, Define.Prefer.norm });
            mbtiQueue.Enqueue(new Define.Prefer[] { Define.Prefer.norm, Define.Prefer.norm, Define.Prefer.like, Define.Prefer.dislike });
        }
    }

    private Define.Prefer[] getMbtis()
    {
        if (mbtiQueue.Count > 0)
        {
            return mbtiQueue.Dequeue();
        }

        return null;
    }


    private int getItems()
    {
        //TODO : 내가 가지고 있는 아이템, 랜덤으로 3개
        return 0;
    }

    private void initNpcs()
    {
        HashSet<int> npcSet = new HashSet<int>();

        while (npcSet.Count < 3)
        {
            int randomIndex = Random.Range(0, Managers.Data.npcList.Count);
            npcSet.Add(randomIndex);
        }

        npcList = new List<int>(npcSet);
    }
}