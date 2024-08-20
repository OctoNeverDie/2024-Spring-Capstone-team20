using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficePlayerManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    public GameObject MyPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        PlayerPrefab = Resources.Load<GameObject>("Prefabs/OfficePlayer");

        if (PlayerPrefab != null)
        {
            //Debug.Log("Player Prefab 로드 성공!");
            Transform playerStart = GameObject.Find("PlayerStart").transform;
            MyPlayer = Instantiate(PlayerPrefab, playerStart.position, playerStart.rotation);
        }
        else
        {
            Debug.LogError("Player Prefab을 로드할 수 없습니다. 경로를 확인하세요.");
        }
    }
}
