using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject NPCPrefab;
    [SerializeField] int NPCCount;
    [SerializeField] int TalkableNPCCount;
    List<Transform> spawnPoints = new List<Transform>();
    List<GameObject> NPCGroup = new List<GameObject>();

    void Start()
    {
        FindSpawnPoints();

        for (int i = 0; i < NPCCount; i++)
        {
            SpawnNPC(i);
        }
    }

    private void FindSpawnPoints()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
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
