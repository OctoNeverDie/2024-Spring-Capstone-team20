using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject NPCPrefab;
    [SerializeField] int NPCCount = 20;
    [SerializeField] int TalkableNPCCount = 5;


    void Awake()
    {
        LoadNPCPrefab();
        
    }

    void Start()
    {
        for (int i = 0; i < NPCCount; i++)
        {
            SpawnNPC(i);
        }
    }

    private void LoadNPCPrefab()
    {
        NPCPrefab = Resources.Load<GameObject>("Prefabs/NPC");

        if (NPCPrefab != null)
        {
            Debug.Log("NPC Prefab 로드 성공!");
        }
        else
        {
            Debug.LogError("NPC Prefab을 로드할 수 없습니다. 경로를 확인하세요.");
        }
    }

    

    private void SpawnNPC(int i)
    {
        Transform npcTransform = Managers.NPC.Move.GetRandomSpawnPoint();
        GameObject newNPC = Instantiate(NPCPrefab, npcTransform.position, npcTransform.rotation);
        newNPC.transform.parent = Managers.NPC.NPCHolder.transform;
        Managers.NPC.NPCGroup.Add(newNPC);

        NPC npcScript = newNPC.GetComponent<NPC>();
        npcScript.destination = Managers.NPC.Move.GetRandomSpawnPoint();

        if (i < TalkableNPCCount)
        {
            npcScript.currentTalkable = NPCDefine.Talkable.Able;
            npcScript.SetTalkable();
        }
        else
        {
            npcScript.currentTalkable = NPCDefine.Talkable.Not;
            npcScript.SetTalkable();
        }
    }


}
