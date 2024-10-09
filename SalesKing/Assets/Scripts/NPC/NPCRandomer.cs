using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC list 랜덤으로 섞고, 마주칠 때마다 Queue로 NPC 뺌
public class NpcSupplyManager : MonoBehaviour
{
    void Start()
    {
        InitQueue();
    }

    private List<NpcInfo> npcSuffledList;
    private Queue<NpcInfo> npcQueue;

    private void InitQueue()
    {
        //TODO : SO로 바꾸기
        npcSuffledList = Managers.Data.npcList;
        for (int i = 0; i < npcSuffledList.Count; i++)
        {
            NpcInfo temp = npcSuffledList[i];
            int randomIdx = Random.Range(i, npcSuffledList.Count);
            npcSuffledList[i] = npcSuffledList[randomIdx];
            npcSuffledList[randomIdx] = temp;
        }

        npcQueue = new Queue<NpcInfo>(npcSuffledList);
    }

    public NpcInfo GetNextNpc()
    {
        if (npcQueue.Count > 0)
        {
            return npcQueue.Dequeue();
        }
        
        else
        {
            Debug.Log("이제 NPC가 없음");
            return null;
        }
    }
}
