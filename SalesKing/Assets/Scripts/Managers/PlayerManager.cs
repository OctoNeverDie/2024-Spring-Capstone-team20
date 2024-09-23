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

    private void SpawnPlayer()
    {
        PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        if (PlayerPrefab != null)
        {
            Transform playerStart = GameObject.Find("PlayerStart").transform;
            MyPlayer = Instantiate(PlayerPrefab, playerStart.position, playerStart.rotation);
            Debug.Log("PlayerStart Rotation: " + playerStart.rotation.eulerAngles);
        }
    }
}
