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
            Debug.Log("SpawnPoints ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    public Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);

        if (spawnPoints != null) return spawnPoints[index];
        else return null;
    }
}

