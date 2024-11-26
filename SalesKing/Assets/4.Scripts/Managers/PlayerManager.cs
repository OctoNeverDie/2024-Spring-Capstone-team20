using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>, ISingletonSettings
{
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject PlayerSpawnPoint;

    public GameObject MyPlayer;
    public Player player;

    public bool ShouldNotDestroyOnLoad => true;

    protected override void Awake()
    {
        base.Awake();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        //PlayerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        if (PlayerPrefab != null)
        {
            Transform playerStart = PlayerSpawnPoint.transform;
            MyPlayer = Instantiate(PlayerPrefab, playerStart.position, playerStart.rotation);
            player = MyPlayer.GetComponent<Player>();
        }
    }
}
