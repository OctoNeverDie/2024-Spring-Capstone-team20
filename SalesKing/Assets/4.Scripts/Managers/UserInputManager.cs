using System.Collections.Generic;
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
                if (thisNPC.currentTalkable == NPCDefine.Talkable.Able)
                {
                    ChatManager.Instance.Init(thisNPC.NpcID);

                    myPlayer.PlayerEnterConvo(thisNPC.gameObject);
                    thisNPC.NPCEnterConvo(myPlayer.gameObject);
                }
            }
        }

        if (Input.GetButtonDown("ESC"))
        {
            if (OptionPanel.activeSelf)
            {
                OptionPanel.SetActive(false);
            }
            else
            {
                OptionPanel.SetActive(true);
            }
        }

    }
}
