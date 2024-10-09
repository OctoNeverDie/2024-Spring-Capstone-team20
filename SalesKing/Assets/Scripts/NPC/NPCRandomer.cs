using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC list 랜덤으로 섞고, 마주칠 때마다 Queue로 NPC 뺌
public class NPCRandomer : MonoBehaviour
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
            //int randomIdx = Random.Range()
        }
    }
}
