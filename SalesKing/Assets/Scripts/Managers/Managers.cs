using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    private static Managers instance; 
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
    } 

    public GameObject ManagersGO;

    SceneModeManager _scene;
    NPCManager _npc;
    UIManager _ui;
    PlayerManager _player;
    TurnManager _turn;
    CameraManager _cam;
    DataManager _data = new DataManager();
    GPTManager _gpt;
    OfficeManager _office;

    public static SceneModeManager Scene { get { return Instance._scene; } }
    public static NPCManager NPC { get { return Instance._npc; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static PlayerManager Player { get { return Instance._player; } }
    public static TurnManager Turn { get { return Instance._turn; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static DataManager Data { get { return Instance._data;  } }
    public static GPTManager GPT { get { return Instance._gpt; } }
    public static OfficeManager Office { get { return Instance._office; } }

    void Awake()
    {
        Init();
        ManagersGO = transform.gameObject;

        if (instance._scene == null)
        {
            instance._scene = ManagersGO.AddComponent<SceneModeManager>();
        }
    }

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

        instance._data.Init();
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

    public void AddTurnManager()
    {
        GameObject turnManager = new GameObject("@TurnManager");
        turnManager.transform.parent = transform;

        if (instance._turn == null)
        {
            instance._turn = turnManager.AddComponent<TurnManager>();
        }
    }

    public void AddCameraManager()
    {
        GameObject camManager = new GameObject("@CameraManager");
        camManager.transform.parent = transform;

        if (instance._cam == null)
        {
            instance._cam = camManager.AddComponent<CameraManager>();
        }
    }

    public void AddGPTManager()
    {
        GameObject gptManager = new GameObject("@GPTManager");
        gptManager.transform.parent = transform;

        if (instance._gpt == null)
        {
            instance._gpt = gptManager.AddComponent<GPTManager>();
        }
    }

    public void AddOfficeManager()
    {
        GameObject officeManager = new GameObject("@OfficeManager");
        officeManager.transform.parent = transform;

        if (instance._office == null)
        {
            instance._office = officeManager.AddComponent<OfficeManager>();
        }
    }

    public void ClearChildManagers()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}