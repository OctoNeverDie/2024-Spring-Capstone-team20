using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject NPCPrefab;

    private void LoadNPCPrefab()
    {
        NPCPrefab = Resources.Load<GameObject>("Prefabs/NPC");

        if (NPCPrefab != null)
        {
            //Debug.Log("NPC Prefab 로드 성공!");
        }
        else
        {
            Debug.LogError("NPC Prefab을 로드할 수 없습니다. 경로를 확인하세요.");
        }
    }



}
