using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, ISingletonSettings
{
    [SerializeField] GameObject PlayerPrefab;
    public GameObject MyPlayer;

    public bool ShouldNotDestroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
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
