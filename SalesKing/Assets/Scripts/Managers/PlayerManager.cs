using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    public GameObject MyPlayer;

    void Start()
    {
        SpawnPlayer();
    }

    void Update()
    {
        
    }

    private void SpawnPlayer()
    {
        PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        if (PlayerPrefab != null)
        {
            //Debug.Log("Player Prefab �ε� ����!");
            Transform playerStart = GameObject.Find("PlayerStart").transform;
            MyPlayer = Instantiate(PlayerPrefab, playerStart.position, playerStart.rotation);
        }
        else
        {
            Debug.LogError("Player Prefab�� �ε��� �� �����ϴ�. ��θ� Ȯ���ϼ���.");
        }
    }
}
