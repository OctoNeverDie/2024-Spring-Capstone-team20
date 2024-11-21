using UnityEngine;
/// <summary>
/// player에 붙어있음!
/// </summary>
public class UserInputManager : Singleton<UserInputManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;
    public bool isKeyInputLocked = false;

    Player myPlayer;

    [SerializeField] Define.UserInputMode DefaultMode = Define.UserInputMode.Voice;
    [HideInInspector]
    public Define.UserInputMode CurInputMode;

    void Start()
    {
        myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();
        CurInputMode = DefaultMode;
        //if(Managers.Scene.curScene==Define.SceneMode.CityMap) Managers.UI.InitiateInputMode();
    }

    void Update()
    {
        if (!isKeyInputLocked && Input.GetButtonDown("Interaction"))
        {
            if(myPlayer == null)
            {
                myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();
            }

            if (myPlayer.ui.RaycastHitObj.activeSelf)
            {
                NPC thisNPC = myPlayer.RaycastCollider.GetComponent<NPC>();
                ChatManager.Instance.Init(thisNPC.NpcID);
                
                myPlayer.PlayerEnterConvo(thisNPC.gameObject);
                thisNPC.NPCEnterConvo(myPlayer.gameObject);
            }
        }

    }
}
