using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC list 랜덤으로 섞고, 마주칠 때마다 Queue로 NPC 뺌
public class NpcSupplyManager
{
    private List<NpcInfo> npcSuffledList;
    private Queue<NpcInfo> npcQueue;

    public void InitQueue()
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
        PrintNpcQueue();
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

    public void PrintNpcQueue()
    {
        if (npcQueue.Count == 0)
        {
            Debug.Log("NPC Queue is empty.");
            return;
        }

        int index = 1;
        foreach (NpcInfo npc in npcQueue)
        {
            Debug.Log($"NPC {index}: " +
                      $"ID = {npc.NpcID}, " +
                      $"Name = {npc.NpcName}, " +
                      $"Sex = {npc.NpcSex}, " +
                      $"Age = {npc.NpcAge}, " +
                      $"Situation = {npc.Situation_Description}, " +
                      $"Personality = {npc.Personality}, " +
                      $"Dialogue Style = {npc.Dialogue_Style}");
            index++;
        }
    }

}