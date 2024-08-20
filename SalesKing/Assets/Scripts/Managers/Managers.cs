using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance; // 유일성이 보장된다
    public static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    NPCManager _npc;
    UIManager _ui;
    PlayerManager _player;

    public static NPCManager NPC { get { return Instance._npc; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static PlayerManager Player { get { return Instance._player; } }

    void Awake()
    {
        Init();

        GameObject npcManager = new GameObject("@NPCManager");
        npcManager.transform.parent = transform;

        if (s_instance._npc == null)
        {
            s_instance._npc = npcManager.AddComponent<NPCManager>();
        }

        GameObject uiManager = new GameObject("@UIManager");
        uiManager.transform.parent = transform;

        if (s_instance._ui == null)
        {
            s_instance._ui = uiManager.AddComponent<UIManager>();
        }

        GameObject playerManager = new GameObject("@PlayerManager");
        playerManager.transform.parent = transform;

        if (s_instance._player == null)
        {
            s_instance._player = playerManager.AddComponent<PlayerManager>();
        }
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}