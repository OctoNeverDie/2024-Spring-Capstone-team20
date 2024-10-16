using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariableInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField text;
    private string _userInput="";

    public void OnClick()
    {
        if(text == null)
        {
            Debug.Log("No inputfield");
            return;
        }
        _userInput = text.text;
        TutorialManager.Instance.OnPersuadeToCustomer();
        //TODO : 현재 단계가 chatSaleState나, ChatBargain 단계 아니면 작성하지 못하게 하기.
        Managers.Chat.ReplyManager.UserAnswer = _userInput;
        text.text = "";

        if(Managers.Input.CurInputMode == Define.UserInputMode.Voice)
        {
            STTUI _sttUI = FindObjectOfType<STTUI>().GetComponent<STTUI>();
            _sttUI.OnClickEnter();
        }

    }
}
