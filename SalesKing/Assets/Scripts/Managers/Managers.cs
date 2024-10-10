using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
//using TMPro.EditorUtilities;
//using UnityEditor.EditorTools;
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
    
    // 재산 관련
    CashManager _cash;
    //인벤토리 관련
    InventoryManager _inven;

    MissionManager _mission;
    //대화 관련
    ChatManager _chat;

    UserInputManager _input;

    TransitionManager _trans;

    TurnManager _turn;

    TutorialManager _tutorial;

    public static SceneModeManager Scene { get { return Instance._scene; } }
    public static NPCManager NPC { get { return Instance._npc; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static PlayerManager Player { get { return Instance._player; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static CameraManager Cam { get { return Instance._cam; } }
    public static DataManager Data { get { return Instance._data;  } }
    public static OfficeManager Office { get { return Instance._office; } }
    public static ConvoManager Convo { get { return Instance._convo; } }
    public static CashManager Cash { get { return Instance._cash; } }
    public static InventoryManager Inven { get { return Instance._inven; } }
    public static MissionManager Mission { get { return Instance._mission; } }
    public static ChatManager Chat { get { return Instance._chat; } }
    public static UserInputManager Input { get { return Instance._input; } }
    public static TransitionManager Trans { get { return Instance._trans; } }
    public static TurnManager Turn { get { return Instance._turn; } }
    public static TutorialManager Tutorial { get { return Instance._tutorial; } }

    void Awake()
    {
        Init();
        ManagersGO = transform.gameObject;

        if (instance._scene == null)
        {
            instance._scene = ManagersGO.AddComponent<SceneModeManager>();
        }
        if (instance._mission == null)
        {
            instance._mission = ManagersGO.AddComponent<MissionManager>();
        }
        if (instance._input == null)
        {
            instance._input = ManagersGO.AddComponent<UserInputManager>();
        }
        if (instance._trans == null)
        {
            instance._trans = ManagersGO.AddComponent<TransitionManager>();
        }
        if (instance._tutorial == null)
        {
            instance._tutorial = ManagersGO.AddComponent<TutorialManager>();
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

    public void AddCashManager()
    {
        GameObject cashManager = new GameObject("@CashManager");
        cashManager.transform.parent = transform;

        if (instance._cash == null)
        {
            instance._cash = cashManager.AddComponent<CashManager>();
        }
    }

    public void AddChatManager()
    {
        GameObject chatManager = new GameObject("@ChatManager");
        chatManager.transform.parent = transform;

        if (instance._chat == null)
        {
            instance._chat = chatManager.AddComponent<ChatManager>();
            instance._chat.npcSupplyManager.InitQueue();
        }
    }

    public void AddInventoryManager()
    {
        GameObject inventoryManager = new GameObject("@InventoryManager");
        inventoryManager.transform.parent = transform;

        if (instance._inven == null)
        {
            instance._inven = inventoryManager.AddComponent<InventoryManager>();
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

    public void ClearChildManagers()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}