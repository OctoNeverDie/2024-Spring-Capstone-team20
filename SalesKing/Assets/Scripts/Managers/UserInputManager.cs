using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween 사용
/// <summary>
/// player에 붙어있음!
/// </summary>
public class UserInputManager : Singleton<UserInputManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    Player myPlayer;

    [SerializeField] Define.UserInputMode DefaultMode = Define.UserInputMode.Keyboard;
    [SerializeField]
    [HideInInspector]
    public Define.UserInputMode CurInputMode;
    private VariableInput _variableInput;

    void Start()
    {
        _variableInput = GetComponent<VariableInput>();
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
                NPC thisNPC = myPlayer.RaycastCollider.GetComponent<NPC>();
                ChatManager.Instance.Init(thisNPC.NpcID);
                
                myPlayer.PlayerEnterConvo(thisNPC.gameObject);
                thisNPC.NPCEnterConvo(myPlayer.gameObject);
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
