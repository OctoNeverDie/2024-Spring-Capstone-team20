using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject NPC;
    [SerializeField] int NPC_count;
    GameObject locationGroup;
    List<Transform> spawnPoints = new List<Transform>();
    List<GameObject> NPCGroup = new List<GameObject>();

    void Start()
    {
        findSpawnPoints();
        
        for(int i=0; i<NPC_count; i++)
        {
            spawnNPC();
        }
    }

    private void findSpawnPoints()
    {
        locationGroup = transform.gameObject;
        int n = locationGroup.transform.childCount;
        for(int i=0; i < n; i++)
        {
            // 맵에 있는 위치들 불러오기
            spawnPoints.Add(locationGroup.transform.GetChild(i).transform);
        }
    }

    private void spawnNPC()
    {
        // 랜덤한 생성 포인트
        int pos1 = Random.Range(0, spawnPoints.Count);
        // 랜덤한 목적지 포인트
        int pos2 = Random.Range(0, spawnPoints.Count);

        GameObject newNPC = Instantiate(NPC, spawnPoints[pos1].position, spawnPoints[pos1].rotation);
        NPCGroup.Add(newNPC);
        newNPC.GetComponent<NPC>().destination = spawnPoints[pos2];
    }


}
