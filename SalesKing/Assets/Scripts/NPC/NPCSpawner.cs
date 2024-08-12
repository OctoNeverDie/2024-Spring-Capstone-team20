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
            Debug.Log("NPC Prefab �ε� ����!");
        }
        else
        {
            Debug.LogError("NPC Prefab�� �ε��� �� �����ϴ�. ��θ� Ȯ���ϼ���.");
        }
    }

    private void FindSpawnPoints()
    {
        // "SpawnPoints"��� �̸��� ���� ������Ʈ�� ã���ϴ�.
        GameObject GO = GameObject.Find("SpawnPoints");

        // spawnPoints ������Ʈ�� �����ϴ��� Ȯ���մϴ�.
        if (GO != null)
        {
            // spawnPoints ������Ʈ�� ��� �ڽ��� ��ȸ�մϴ�.
            foreach (Transform child in GO.transform)
            {
                // �ڽ� Transform�� spawnPointList�� �߰��մϴ�.
                spawnPoints.Add(child);
            }
        }
        else
        {
            Debug.LogError("SpawnPoints ������Ʈ�� ã�� �� �����ϴ�.");
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
