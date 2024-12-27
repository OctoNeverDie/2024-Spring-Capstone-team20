using UnityEngine;
/// <summary>
/// player에 붙어있음!
/// </summary>
public class UserInputManager : Singleton<UserInputManager>, ISingletonSettings
{
    [HideInInspector]
    public bool ShouldNotDestroyOnLoad => false;
    public bool isKeyInputLocked = false;

    [SerializeField] private GameObject OptionPanel;
    Player myPlayer;

    [SerializeField] Define.UserInputMode DefaultMode = Define.UserInputMode.Voice;
    [HideInInspector]
    public Define.UserInputMode CurInputMode;

    void Start()
    {
        myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();
        CurInputMode = DefaultMode;
    }

    void Update()
    {
        /*
        if (!isKeyInputLocked && Input.GetButtonDown("Interaction"))
        {
            if(myPlayer == null)
            {
                myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();
            }

            if (myPlayer.ui.RaycastHitObj.activeSelf)
            {
                NPC thisNPC = myPlayer.RaycastCollider.GetComponent<NPC>();
                if (thisNPC.Talkable)
                {
                    thisNPC.Talkable = false;
                    ChatManager.Instance.Init(thisNPC.NpcID);

                    myPlayer.PlayerEnterConvo(thisNPC.gameObject);
                    thisNPC.NPCEnterConvo(myPlayer.gameObject);
                }
            }
        }
        */
        if (Input.GetButtonDown("ESC"))
        {
            if (MuhanNpcDataManager.Instance != null)
                return;

            if (OptionPanel.activeSelf)
            {
                OptionPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                OptionPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
