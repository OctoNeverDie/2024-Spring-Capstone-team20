using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    private static Managers instance; // 유일성이 보장된다
    public static Managers Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Managers>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<Managers>();
                    singleton.name = typeof(Managers).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    } // 유일한 매니저를 갖고온다

    public GameObject ManagersGO;

    SceneModeManager _scene;
    NPCManager _npc;
    UIManager _ui;
    PlayerManager _player;

    public static SceneModeManager Scene { get { return Instance._scene; } }
    public static NPCManager NPC { get { return Instance._npc; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static PlayerManager Player { get { return Instance._player; } }

    void Awake()
    {
        Init();
        ManagersGO = transform.gameObject;


        if (instance._scene == null)
        {
            instance._scene = ManagersGO.AddComponent<SceneModeManager>();
        }
    }

    // Managers 깨우기
    void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayerManager()
    {
        GameObject playerManager = new GameObject("@PlayerManager");
        playerManager.transform.parent = transform;

        if (instance._player == null)
        {
            instance._player = playerManager.AddComponent<PlayerManager>();
        }
    }

    public void AddNPCManager()
    {
        GameObject npcManager = new GameObject("@NPCManager");
        npcManager.transform.parent = transform;

        if (instance._npc == null)
        {
            instance._npc = npcManager.AddComponent<NPCManager>();
        }
    }

    public void AddUIManager()
    {
        GameObject uiManager = new GameObject("@UIManager");
        uiManager.transform.parent = transform;

        if (instance._ui == null)
        {
            instance._ui = uiManager.AddComponent<UIManager>();
        }
    }
    
}