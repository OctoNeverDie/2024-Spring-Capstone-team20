using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManagers : MonoBehaviour
{
    private static OfficeManagers s_instance; // 유일성이 보장된다
    public static OfficeManagers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    OfficePlayerManager _player;

    public static OfficePlayerManager Player { get { return Instance._player; } }

    void Awake()
    {
        Init();

        GameObject playerManager = new GameObject("@PlayerManager");
        playerManager.transform.parent = transform;

        if (s_instance._player == null)
        {
            s_instance._player = playerManager.AddComponent<OfficePlayerManager>();
        }
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@OfficeManager");
            if (go == null)
            {
                go = new GameObject { name = "@OfficeManager" };
                go.AddComponent<OfficeManagers>();
            }

            //DontDestroyOnLoad(go);
            s_instance = go.GetComponent<OfficeManagers>();
        }
    }
}
