using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 사용

public class UserInputManager : MonoBehaviour
{
    Player myPlayer;

    public Define.UserInputMode DefaultMode = Define.UserInputMode.Keyboard;
    public Define.UserInputMode CurInputMode;
    private VariableInput _variableInput;

    void Start()
    {
        _variableInput = FindObjectOfType<VariableInput>();
        myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();
        CurInputMode = DefaultMode;
        //if(Managers.Scene.curScene==Define.SceneMode.CityMap) Managers.UI.InitiateInputMode();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            if(myPlayer == null)
            {
                myPlayer = PlayerManager.Instance.MyPlayer.GetComponent<Player>();
            }

            if (myPlayer.ui.RaycastHitObj.activeSelf)
            {
                switch (myPlayer.ui.curInteractable)
                {
                    //case Define.Interactables.Office_MyPC: Managers.Office.officeUI.OnClickMyPC(); break;
                    /*
                    case Define.Interactables.Office_Door_Out:
                        //Managers.Trans.ui.StartFadeInFadeOut(1f);
                        DOVirtual.DelayedCall(Managers.Trans.ui.FadeTime, () => Managers.Scene.LoadSceneByName("CityMap"));
                        break;
                    */
                    case Define.Interactables.City_NPC:
                        NPC thisNPC = myPlayer.RaycastCollider.GetComponent<NPC>();
                        //Managers.Chat.EvalManager.currentNpcId= thisNPC.NpcID;
                        myPlayer.PlayerEnterConvo(thisNPC.gameObject);
                        thisNPC.NPCEnterConvo(myPlayer.gameObject);
                        break;
                        /*
                    case Define.Interactables.Office_Secretary:
                        Debug.Log("비서한테 말걸기");
                        Secretary thisSecretary = myPlayer.RaycastCollider.GetComponent<Secretary>();
                        thisSecretary.ShowPanel();
                        break;
                        */
                    default: break;
                }

            }
        }

        if (Input.GetButtonDown("Submit"))
        {
            if (_variableInput == null)
            {
                Debug.Log("Null exception : variableInput이 비었습니다. ");
                _variableInput = FindObjectOfType<VariableInput>();
            }
            _variableInput.OnClick();
        }
    }
}
