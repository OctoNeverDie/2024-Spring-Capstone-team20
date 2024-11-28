using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    private List<Transform> spawn_points = new List<Transform>();
    
    void Awake()
    {
        GameObject spawn_points_holder = GameObject.Find("SpawnPoints");
        // Null 체크
        if (spawn_points_holder != null)
        {
            // 자식 오브젝트의 Transform 모두 가져오기
            foreach (Transform child in spawn_points_holder.transform)
            {
                spawn_points.Add(child); // 리스트에 추가
            }
            SpawnStoryModeNPCs();
        }

        
    }
        
    public void SpawnStoryModeNPCs()
    {
        int stage_num = DataController.Instance.playData.cur_day_ID;
        StoryNpcSO story_so = NPCManager.Instance.storyNpcSO;
        NpcLookSO looks_so = NPCManager.Instance.npcLookSO;

        // 이 스테이지의 npc 3개
        for (int i=0; i < story_so.storyNpcs[stage_num].npc_IDs.Count; i++)
        {
            // 각각의 npc ID
            int npc_ID = story_so.storyNpcs[stage_num].npc_IDs[i];

            // npc mesh 가져오기
            GameObject npc_body = null;
            for (int j=0; j< looks_so.npcLooks.Count; j++)
            {
                if (looks_so.npcLooks[j].npcId == npc_ID)
                {
                    npc_body = looks_so.npcLooks[j].look;
                    break;
                }
            }
            
            Transform npc_spawn_point = spawn_points[i];
            GameObject spawned_npc = Instantiate(NPCManager.Instance.NPCPrefab, npc_spawn_point.position, npc_spawn_point.rotation);
            Instantiate(npc_body, spawned_npc.transform);
            spawned_npc.GetComponent<NPC>().NpcID = npc_ID;
        }
    }

    public void SpawnInfiniteModeNPCs()
    {

    }
}
