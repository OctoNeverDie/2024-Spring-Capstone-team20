using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 사용

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
            if(myPlayer == null)
            {
                myPlayer = Managers.Player.MyPlayer.GetComponent<Player>();
            } 

            if (myPlayer.ui.RaycastHitObj.activeSelf)
            {
                switch (myPlayer.ui.curInteractable)
                {
                    case Define.Interactables.Office_MyPC: Managers.Office.officeUI.OnClickMyPC(); break;
                    case Define.Interactables.Office_Door_Out:
                        Managers.Trans.ui.StartFadeInFadeOut(1f);
                        DOVirtual.DelayedCall(Managers.Trans.ui.FadeTime, () => Managers.Scene.LoadSceneByName("CityMap"));
                        break;

                    case Define.Interactables.City_NPC:
                        NPC thisNPC = myPlayer.RaycastCollider.GetComponent<NPC>();
                        TutorialManager.Instance.OnTalkToCustomer();
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
