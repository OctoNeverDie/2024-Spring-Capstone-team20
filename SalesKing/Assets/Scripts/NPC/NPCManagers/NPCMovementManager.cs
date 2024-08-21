using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : MonoBehaviour
{
    List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        FindSpawnPoints();
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
}

