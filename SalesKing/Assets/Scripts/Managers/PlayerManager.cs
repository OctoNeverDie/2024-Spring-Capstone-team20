using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    public GameObject MyPlayer;

    void Awake()
    {
        SpawnPlayer();
        //Managers.Input.myPlayer = MyPlayer.GetComponent<Player>();
    }

    private void SpawnPlayer()
    {
        PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        if (PlayerPrefab != null)
        {
            Transform playerStart = GameObject.Find("PlayerStart").transform;
            MyPlayer = Instantiate(PlayerPrefab, playerStart.position, playerStart.rotation);
        }
    }
}
