using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public int cur_NPC_index = 0;
    public bool check_for_tuto_end = false;

    void Awake()
    {
        cur_NPC_index = 0;
        check_for_tuto_end = false;
    }

    void Update()
    {
        if (check_for_tuto_end)
        {
            if (TutorialManager.Instance != null && TutorialManager.Instance.controller != null && TutorialManager.Instance.controller.isComplete)
            {
                SpawnNextNPC();
                check_for_tuto_end = false;
            }
        }
    }

    public void SpawnNextNPC()
    {
        if(cur_NPC_index < 3)
        {
            SpawnStoryModeNPCs(cur_NPC_index);
        }
    }

    void SpawnStoryModeNPCs(int npc_index)
    {
        int stage_num = DataController.Instance.gameData.cur_day_ID;
        StoryNpcSO story_so = NPCManager.Instance.storyNpcSO;
        NpcLookSO looks_so = NPCManager.Instance.npcLookSO;

        // 각각의 npc ID
        int npc_ID = story_so.storyNpcs[stage_num].npc_IDs[npc_index];

        // npc mesh 가져오기
        GameObject npc_body = null;
        for (int j = 0; j < looks_so.npcLooks.Count; j++)
        {
            if (looks_so.npcLooks[j].npcId == npc_ID)
            {
                npc_body = looks_so.npcLooks[j].look;
                break;
            }
        }

        Transform npc_spawn_point = NPCManager.Instance.SpawnPoint;
        GameObject spawned_npc = Instantiate(NPCManager.Instance.NPCPrefab, npc_spawn_point.position, npc_spawn_point.rotation);
        Instantiate(npc_body, spawned_npc.transform);

        NPC npc = spawned_npc.GetComponent<NPC>();
        npc.NpcID = npc_ID;
        npc.SetNPCDestination(NPCManager.Instance.StandPoint.position, true);
        cur_NPC_index++;

        /*
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

            Transform npc_spawn_point = NPCManager.Instance.SpawnPoint;
            GameObject spawned_npc = Instantiate(NPCManager.Instance.NPCPrefab, npc_spawn_point.position, npc_spawn_point.rotation);
            Instantiate(npc_body, spawned_npc.transform);
            spawned_npc.GetComponent<NPC>().NpcID = npc_ID;
        }
        */
    }
}
