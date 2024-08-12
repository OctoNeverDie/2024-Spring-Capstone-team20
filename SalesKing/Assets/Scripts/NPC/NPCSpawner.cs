using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject NPCPrefab;
    [SerializeField] int NPCCount = 20;
    [SerializeField] int TalkableNPCCount = 5;
    List<Transform> spawnPoints = new List<Transform>();
    List<GameObject> NPCGroup = new List<GameObject>();

    void Start()
    {
        LoadNPCPrefab();
        FindSpawnPoints();

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

    private void FindSpawnPoints()
    {
        // "SpawnPoints"라는 이름을 가진 오브젝트를 찾습니다.
        GameObject GO = GameObject.Find("SpawnPoints");

        // spawnPoints 오브젝트가 존재하는지 확인합니다.
        if (GO != null)
        {
            // spawnPoints 오브젝트의 모든 자식을 순회합니다.
            foreach (Transform child in GO.transform)
            {
                // 자식 Transform을 spawnPointList에 추가합니다.
                spawnPoints.Add(child);
            }
        }
        else
        {
            Debug.LogError("SpawnPoints 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void SpawnNPC(int i)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        GameObject newNPC = Instantiate(NPCPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        NPCGroup.Add(newNPC);

        NPC npcScript = newNPC.GetComponent<NPC>();
        npcScript.destination = GetRandomSpawnPoint();

        if (i < TalkableNPCCount)
        {
            npcScript.currentTalkable = NPC.Talkable.Able;
        }
        else
        {
            npcScript.currentTalkable = NPC.Talkable.Not;
        }
    }

    public Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }
}
