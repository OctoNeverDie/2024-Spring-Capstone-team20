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

    // 씬마다 다른 매니저 넣어주기
    SceneModeManager _scene;

    // npc와 player
    NPCManager _npc;
    PlayerManager _player;

    // ui 오브젝트에 접근하는 코드들
    UIManager _ui;
    
    // 시간 가는 연출, 카메라 워킹 등
    TimeManager _time;
    CameraManager _cam;

    // 찐 중요한 부분. 메인 핵심.
    ConvoManager _convo;

    DataManager _data = new DataManager();
    
    // office에서 쓰는 것들
    OfficeManager _office;
    

    public static SceneModeManager Scene { get { return Instance._scene; } }
    public static NPCManager NPC { get { return Instance._npc; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static PlayerManager Player { get { return Instance._player; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static DataManager Data { get { return Instance._data;  } }
    public static OfficeManager Office { get { return Instance._office; } }
    public static ConvoManager Convo { get { return Instance._convo; } }

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

    public void AddTimeManager()
    {
        GameObject timeManager = new GameObject("@TimeManager");
        timeManager.transform.parent = transform;

        if (instance._time == null)
        {
            instance._time = timeManager.AddComponent<TimeManager>();
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

    public void AddOfficeManager()
    {
        GameObject officeManager = new GameObject("@OfficeManager");
        officeManager.transform.parent = transform;

        if (instance._office == null)
        {
            instance._office = officeManager.AddComponent<OfficeManager>();
        }
    }

    public void AddConvoManager()
    {
        GameObject convoManager = new GameObject("@ConvoManager");
        convoManager.transform.parent = transform;

        if (instance._convo == null)
        {
            instance._convo = convoManager.AddComponent<ConvoManager>();
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