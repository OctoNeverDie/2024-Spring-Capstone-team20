using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    Player myPlayer;
    void Start()
    {
        myPlayer = Managers.Player.MyPlayer.GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            if (myPlayer.ui.RaycastHitObj.activeSelf)
            {
                switch (myPlayer.ui.curInteractable)
                {
                    case Define.Interactables.Office_MyPC: Managers.Office.officeUI.OnClickMyPC(); break;
                    case Define.Interactables.Office_Door_Out: 
                        Managers.Scene.LoadSceneByName("CityMap");
                        myPlayer.ui.StartFadeInFadeOut(3f);
                        break;

                    case Define.Interactables.City_NPC:
                        NPC thisNPC = myPlayer.RaycastCollider.GetComponent<NPC>();
                        Managers.Chat.Init();
                        myPlayer.PlayerEnterConvo(thisNPC.gameObject);
                        thisNPC.NPCEnterConvo(myPlayer.gameObject);
                        break;

                    default: break;
                }

            }
        }
    }
}
