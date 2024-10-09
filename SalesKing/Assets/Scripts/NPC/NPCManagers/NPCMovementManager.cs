using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : MonoBehaviour
{
    List<Transform> spawnPoints = new List<Transform>();
    List<Transform> availableSpawnPoints = new List<Transform>();
    List<Transform> usedSpawnPoints = new List<Transform>();

    private void Awake()
    {
        FindSpawnPoints();
        availableSpawnPoints = spawnPoints;
    }

    private void FindSpawnPoints()
    {
        GameObject GO = GameObject.Find("SpawnPoints");

        if (GO != null)
        {
            foreach (Transform child in GO.transform)
            {
                spawnPoints.Add(child);
            }
        }
        else
        {
            Debug.Log("SpawnPoints 오브젝트를 찾을 수 없습니다.");
        }
    }

    public Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);

        if (spawnPoints != null) return spawnPoints[index];
        else return null;
    }

    public Transform GetUniqueSpawnPoint()
    {
        if (availableSpawnPoints.Count == 0)
        {
            //Debug.LogError("No more available spawn points.");
            return null;
        }

        // 랜덤하게 스폰 포인트 선택
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[randomIndex];

        // 선택된 스폰 포인트는 available 리스트에서 제거하고, used 리스트에 추가
        availableSpawnPoints.RemoveAt(randomIndex);
        usedSpawnPoints.Add(spawnPoint);

        return spawnPoint;
    }
}

