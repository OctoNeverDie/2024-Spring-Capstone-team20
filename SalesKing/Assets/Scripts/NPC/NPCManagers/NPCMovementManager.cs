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

