using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : MonoBehaviour
{

    List<Transform> spawnPoints = new List<Transform>();

    void Awake()
    {
        FindSpawnPoints();
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

    public Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }
}

